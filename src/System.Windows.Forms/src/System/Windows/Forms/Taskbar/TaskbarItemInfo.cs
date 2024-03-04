// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Drawing;
using Windows.Win32.System.Com;

namespace System.Windows.Forms;

public unsafe class TaskbarItemInfo : IDisposable
{
    private AgileComPointer<ITaskbarList4>? _taskbarList;
    private readonly HWND _ownerHandle;

    private TaskbarItemProgressState _progressState;
    private double _progressValue;
    private string? _toolTip;
    private THUMBBUTTON[]? _nativeThumbButtons;

    /// <summary>
    ///  
    /// </summary>
    /// <param name="handle"></param>
    public TaskbarItemInfo(nint handle)
    {
        _ownerHandle = (HWND)handle;
        PInvokeCore.CoCreateInstance(
            CLSID.TaskbarList,
            pUnkOuter: null,
            CLSCTX.CLSCTX_INPROC_SERVER,
            out ITaskbarList4* taskbarList).ThrowOnFailure();
        HRESULT hr = taskbarList->HrInit();
        if (hr.Failed)
        {
            taskbarList->Release();
            hr.ThrowOnFailure();
        }

        hr = taskbarList->ActivateTab(_ownerHandle);
        if (hr.Failed)
        {
            taskbarList->Release();
            hr.ThrowOnFailure();
        }

        _taskbarList = new(taskbarList, takeOwnership: true);
    }

    /// <summary>
    ///  Gets or sets the type and state of the progress indicator displayed on a taskbar button.
    /// </summary>
    public TaskbarItemProgressState ProgressState
    {
        get => _progressState;

        set
        {
            if (_progressState == value)
            {
                return;
            }

            using var taskbarList = _taskbarList!.GetInterface<ITaskbarList4>();
            HRESULT result = taskbarList.Value->SetProgressState(_ownerHandle, (TBPFLAG)value);
            Debug.Assert(result.Succeeded);
            _progressState = value;
        }
    }

    /// <summary>
    ///  Displays or updates a progress bar hosted in a taskbar button to show the specific percentage completed
    ///  of the full operation. The progress value may only be between 0 and 1.
    /// </summary>
    public double ProgressValue
    {
        get => _progressValue;

        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 0);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 1);

            using var taskbarList = _taskbarList!.GetInterface<ITaskbarList4>();
            HRESULT result = taskbarList.Value->SetProgressValue(_ownerHandle, (ulong)(value * 100), 100);
            Debug.Assert(result.Succeeded);
            _progressValue = value;
        }
    }

    /// <summary>
    ///  Gets or sets the text of the tooltip that is displayed when the mouse pointer rests
    ///  on an individual preview thumbnail in a taskbar button flyout.
    /// </summary>
    public string? ToolTip
    {
        get => _toolTip;

        set
        {
            if (_toolTip == value)
            {
                return;
            }

            // If >= 260, automatically takes on the text associated with the control. Do we want to throw...?
            // ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value?.Length ?? 0, 260);
            using var taskbarList = _taskbarList!.GetInterface<ITaskbarList4>();
            taskbarList.Value->SetThumbnailTooltip(_ownerHandle, value).ThrowOnFailure();
            _toolTip = value;
        }
    }

    /// <summary>
    ///  The <see cref="Icon"/> to be displayed as the overlay icon for the taskbar item.
    /// </summary>
    public Icon? OverlayIcon { get; private set; }

    /// <summary>
    ///  The alt text version of the information conveyed by the overlay, if any, for accessibility purposes.
    /// </summary>
    public string? OverlayIconDescription { get; private set; }

    /// <summary>
    ///  Applies an overlay to a taskbar button to indicate application status or a notification to the user.
    /// </summary>
    /// <param name="icon">
    ///  The <see cref="Icon"/> to be displayed as the overlay icon. This should be a small icon, measuring 16x16 pixels at 96 dpi.
    ///  If an overlay icon is already applied to the taskbar button, that existing overlay is replaced.
    ///  A <see langword="null"/> value will remove the overlay icon from the display.
    /// </param>
    /// <param name="description">The alt text version of the information conveyed by the overlay, for accessibility purposes.</param>
    public void SetOverlayIcon(Icon? icon, string? description = null)
    {
        if (OverlayIcon == icon)
        {
            return;
        }

        using var taskbarList = _taskbarList!.GetInterface<ITaskbarList4>();
        taskbarList.Value->SetOverlayIcon(
            _ownerHandle,
            icon?.Handle is nint handle ? (HICON)handle : HICON.Null,
            description).ThrowOnFailure();
        OverlayIcon = icon;
        OverlayIconDescription = description;
    }

    public ThumbButtonInfo[]? ThumbButtonInfos
    {
        get
        {
            if (_nativeThumbButtons is null)
            {
                return null;
            }

            ThumbButtonInfo[] thumbButtonInfos = new ThumbButtonInfo[_nativeThumbButtons.Length];
            for (int i = 0; i < _nativeThumbButtons.Length; i++)
            {
                THUMBBUTTON native = _nativeThumbButtons[i];
                thumbButtonInfos[i] = new()
                {
                    dwFlags = (ThumbButtonFlags)native.dwFlags,
                    dwMask = (ThumbButtonMask)native.dwMask,
                    hIcon = (nint)native.hIcon,
                    iId = native.iId,
                    iBitmap = native.iBitmap,
                    szTip = native.szTip.ToString(),
                };
            }

            return thumbButtonInfos;
        }
    }

    /// <summary>
    ///  Adds a set of thumb buttons to a taskbar item. The maximum amount of buttons that can be added is 7.
    /// </summary>
    /// <param name="thumbButtonInfos">
    ///  The <see cref="ThumbButtonInfo"/> to be added to the taskbar item. The <see cref="ThumbButtonInfo.iId"/> must
    ///  be unique for each button in order for the buttons to be updated if needed.
    /// </param>
    /// <returns><see langword="true"/> if setting thumb buttons was successful. Otherwise <see langword="false"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception> if the number of thumb buttons being added is greater than 7.
    /// <exception cref="ArgumentNullException"> if <paramref name="thumbButtonInfos"/> is null. </exception>
    /// <remarks>
    ///  <para>
    ///   This method allows an application to define buttons for an active toolbar control that is embedded in a
    ///   window's taskbar thumbnail preview.This provides access to the window's essential commands without making
    ///   the user restore or activate the window. For example, Windows Media Player might offer standard media transport
    ///   controls such as play, pause, mute, and stop.
    ///  </para>
    ///  <para>
    ///   The toolbar used in the thumbnail is essentially a standard toolbar control. It has a maximum of seven buttons,
    ///   and it is center-aligned, transparent, and displayed in an area beneath the thumbnail rather than covering any
    ///   portion of it. Each button's ID, image, tooltip, and state are defined in a <see cref="ThumbButtonInfo"/> structure,
    ///   which is then passed to the taskbar. The application can then subsequently show, alter, or hide buttons from the
    ///   thumbnail toolbar as required by its current state by calling <see cref="UpdateThumbButtons(ThumbButtonInfo[])"/>.
    ///  </para>
    ///  <para>
    ///   When a button in a thumbnail toolbar is clicked, the window associated with that thumbnail is sent a WM_COMMAND
    ///   message with the HIWORD of its wParam parameter set to THBN_CLICKED and the LOWORD to the button ID.
    ///  </para>
    ///  <para>
    ///   After a toolbar has been added to a thumbnail, buttons can be altered only through
    ///   <see cref="UpdateThumbButtons(ThumbButtonInfo[])"/>. While individual buttons cannot be added or removed,
    ///   they can be shown and hidden through <see cref="UpdateThumbButtons(ThumbButtonInfo[])"/> as needed.
    ///   The toolbar itself cannot be removed without re-creating the window itself.
    /// </para>
    /// <para>
    ///  Because there is a limited amount of space in which to display thumbnails, as well as a constantly changing number of
    ///  thumbnails to display, applications are not guaranteed a specific toolbar size. If display space is low, buttons in the
    ///  toolbar are truncated from right to left as needed. Therefore, an application should prioritize the commands associated
    ///  with its buttons to ensure that those of highest priority are to the left and are therefore least likely to be truncated.
    /// </para>
    /// <para>
    ///  Thumbnail toolbars are displayed only when thumbnails are being displayed. For instance, if a taskbar button
    ///  represents a group with more open windows than there is room to display thumbnails for, the UI reverts to a
    ///  legacy menu rather than thumbnails.
    /// </para>
    /// </remarks>
    public bool AddThumbButtons(params ThumbButtonInfo[] thumbButtonInfos)
    {
        //!!!!!!!!!!! need a way to hook button click, also need to offer image list for bitmap value
        if (_nativeThumbButtons is not null)
        {
            return false;
        }

        ArgumentOutOfRangeException.ThrowIfGreaterThan(thumbButtonInfos.OrThrowIfNull().Length, 7);
        _nativeThumbButtons = new THUMBBUTTON[thumbButtonInfos.Length];
        for (int i = 0; i < thumbButtonInfos.Length; i++)
        {
            ThumbButtonInfo managed = thumbButtonInfos[i].OrThrowIfNull();
            _nativeThumbButtons[i] = new()
            {
                dwFlags = (THUMBBUTTONFLAGS)managed.dwFlags,
                dwMask = (THUMBBUTTONMASK)managed.dwMask,
                hIcon = (HICON)managed.hIcon,
                iId = managed.iId,
                iBitmap = managed.iBitmap,
                szTip = managed.szTip?.Length > 259 ? default : managed.szTip,
            };
        }

        using var taskbarList = _taskbarList!.GetInterface<ITaskbarList4>();
        fixed (THUMBBUTTON* nativeThumbButtons = _nativeThumbButtons)
        {
            return taskbarList.Value->ThumbBarAddButtons(_ownerHandle, (uint)_nativeThumbButtons.Length, nativeThumbButtons).Succeeded;
        }
    }

    /// <summary>
    ///  Shows, enables, disables, or hides buttons in a thumbnail toolbar as required by the window's current state.
    ///  A thumbnail toolbar is a toolbar embedded in a thumbnail image of a window in a taskbar button flyout.
    /// </summary>
    /// <param name="thumbButtonInfos">
    ///  An array of <see cref="ThumbButtonInfo"/> structures. Each <see cref="ThumbButtonInfo"/> defines an individual button.
    ///  If the button already exists (the <see cref="ThumbButtonInfo.iId"/> value is already defined), then that existing
    ///  button is updated with the information provided in the structure
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///  If <paramref name="thumbButtonInfos"/> is <see langword="null"/> or contains <see langword="null"/>.
    /// </exception>
    /// <returns><see langword="true"/> if there were existing buttons that were updated successfully. Otherwise <see langword="false"/>.</returns>
    /// <remarks>
    ///  <para>
    ///   If there are multiple <see cref="ThumbButtonInfo"/> with the same <see cref="ThumbButtonInfo.iId"/> that exist,
    ///   the existing buttons will not be updating even if <paramref name="thumbButtonInfos"/> contains the id of an existing button.
    ///  </para>
    /// </remarks>
    public void UpdateThumbButtons(params ThumbButtonInfo[] thumbButtonInfos)
    {
        if (_nativeThumbButtons is null)
        {
            return;
        }

        bool foundExisting = false;
        for (int i = 0; i < thumbButtonInfos.OrThrowIfNull().Length; i++)
        {
            ThumbButtonInfo managed = thumbButtonInfos[i].OrThrowIfNull();
            for (int j = 0; j < _nativeThumbButtons.Length; j++)
            {
                if (managed.iId == _nativeThumbButtons[j].iId)
                {
                    _nativeThumbButtons[j].dwFlags = (THUMBBUTTONFLAGS)managed.dwFlags;
                    _nativeThumbButtons[j].dwMask = (THUMBBUTTONMASK)managed.dwMask;
                    _nativeThumbButtons[j].hIcon = (HICON)managed.hIcon;
                    _nativeThumbButtons[j].iBitmap = managed.iBitmap;
                    _nativeThumbButtons[j].szTip = managed.szTip?.Length > 259 ? default : managed.szTip;

                    foundExisting = true;
                    break;
                }
            }
        }

        if (foundExisting)
        {
            using var taskbarList = _taskbarList!.GetInterface<ITaskbarList4>();
            fixed (THUMBBUTTON* nativeThumbButtons = _nativeThumbButtons)
            {
                HRESULT result = taskbarList.Value->ThumbBarUpdateButtons(_ownerHandle, (uint)_nativeThumbButtons.Length, nativeThumbButtons);
                Debug.Assert(result.Succeeded);
            }
        }
    }

    protected virtual void Dispose(bool disposing) => DisposeHelper.NullAndDispose(ref _taskbarList);

    ~TaskbarItemInfo()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

/*    public unsafe class ThumbButtonCollection : IReadOnlyList<ThumbButtonInfo>
    {
        private readonly TaskbarItemInfo _owner;
        private ThumbButtonInfo[] _thumbButtonInfos;
        private THUMBBUTTON[] _nativeThumbButtons;
        private int _count;

        public ThumbButtonCollection(TaskbarItemInfo owner)
        {
            _owner = owner.OrThrowIfNull();
            _thumbButtonInfos = new ThumbButtonInfo[7];
            _nativeThumbButtons = new THUMBBUTTON[7];
        }

        public ThumbButtonInfo this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Count => _count;

        public bool IsReadOnly => false;

        public void Add(ThumbButtonInfo item)
        {
            // what happens when there are already 7 and we try to add an 8th?
            item.OrThrowIfNull();
*//*            if (Contains(item))
            {
                return;
            }*//*

            using var taskbarList = _owner._taskbarList!.GetInterface();
            THUMBBUTTON nativeItem = new()
            {
                dwFlags = (THUMBBUTTONFLAGS)item.dwFlags,
                dwMask = (THUMBBUTTONMASK)item.dwMask,
                iId = item.iId,
                iBitmap = item.iBitmap,
                szTip = item.szTip,
            };

            // Need to accommodate based on what happens above
            _nativeThumbButtons[_count] = nativeItem;
            _thumbButtonInfos[_count++] = item;

            fixed (THUMBBUTTON* nativeThumbButtons = _nativeThumbButtons)
            {
                taskbarList.Value->ThumbBarAddButtons(_owner._ownerHandle, (uint)_count, nativeThumbButtons);
            }
        }

        public IEnumerator<ThumbButtonInfo> GetEnumerator() => throw new NotImplementedException();
        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    }*/
}
