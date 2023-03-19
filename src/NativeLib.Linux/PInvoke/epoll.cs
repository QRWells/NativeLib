using System.Runtime.InteropServices;

namespace NativeLib.Linux.PInvoke;

public static unsafe partial class LibC
{
    #region syscalls

    /// <summary>
    ///     Creates an epoll instance.  Returns an fd for the new instance.
    ///     The "size" parameter is a hint specifying the number of file
    ///     descriptors to be associated with the new instance. The fd
    ///     returned by epoll_create() should be closed with close(2).
    /// </summary>
    /// <param name="size">
    ///     Since Linux 2.6.8, this parameter is ignored, but must be greater than zero.
    /// </param>
    /// <returns>
    ///     On success, these system calls return a file descriptor (a
    ///     nonnegative integer). On error, -1 is returned, and errno is set
    ///     to indicate the error.
    /// </returns>
    [DllImport(Constants.LibC, EntryPoint = "epoll_create", SetLastError = true)]
    public static extern int epoll_create(int size);

    /// <summary>
    ///     Same as <see cref="epoll_create(int)" /> but with flags.
    ///     The unused size parameter has been dropped.
    /// </summary>
    /// <param name="flags">
    ///     If flags is 0, then, other than the fact that the obsolete size
    ///     argument is dropped, <see cref="epoll_create1" /> is the same as
    ///     <see cref="epoll_create" />.
    /// </param>
    /// <returns>
    ///     On success, these system calls return a file descriptor (a
    ///     nonnegative integer). On error, -1 is returned, and errno is set
    ///     to indicate the error.
    /// </returns>
    [DllImport(Constants.LibC, EntryPoint = "epoll_create1", SetLastError = true)]
    public static extern int epoll_create1(int flags);

    /// <summary>
    ///     Add, modify, or remove entries in the interest list of the epoll(7) instance referred to by the file descriptor
    ///     epfd.
    ///     It requests that the operation op be performed
    ///     for the target file descriptor, fd.
    /// </summary>
    /// <param name="epfd">The epoll file descriptor.</param>
    /// <param name="op">
    ///     The operation to perform. <see cref="EPOLL_CTL_ADD" /> <see cref="EPOLL_CTL_DEL" /> and
    ///     <see cref="EPOLL_CTL_MOD" />.
    /// </param>
    /// <param name="fd">The target file descriptor.</param>
    /// <param name="ev">Describes the object linked to the file descriptor <see cref="fd" />.</param>
    /// <returns>
    ///     When successful, epoll_ctl() returns zero.  When an error occurs, -1 is returned and errno is set appropriately.
    /// </returns>
    [DllImport(Constants.LibC, EntryPoint = "epoll_ctl", SetLastError = true)]
    public static extern int epoll_ctl(int epfd, int op, int fd, epoll_event* ev);

    /// <summary>
    ///     Wait for events on an epoll instance.
    /// </summary>
    /// <param name="epfd">The epoll file descriptor.</param>
    /// <param name="ev">The events.</param>
    /// <param name="maxevents">The maximum number of events to be returned.</param>
    /// <param name="timeout">The maximum wait time in milliseconds.</param>
    /// <returns></returns>
    [DllImport(Constants.LibC, EntryPoint = "epoll_wait", SetLastError = true)]
    public static extern int epoll_wait(int epfd, epoll_event* ev, int maxevents, int timeout);

    /// <summary>
    ///     Wait for events on an epoll instance.
    ///     This is a thread-safe version of <see cref="epoll_wait(int, epoll_event*, int, int)" />.
    /// </summary>
    /// <param name="epfd">The epoll file descriptor.</param>
    /// <param name="ev">The events.</param>
    /// <param name="maxevents">The maximum number of events to be returned.</param>
    /// <param name="timeout">The maximum wait time in milliseconds.</param>
    /// <param name="sigmask">A signal mask to apply to the thread during the call.</param>
    /// <returns></returns>
    [DllImport(Constants.LibC, EntryPoint = "epoll_pwait", SetLastError = true)]
    public static extern int epoll_pwait(int epfd, epoll_event* ev, int maxevents, int timeout, void* sigmask);

    #endregion

    #region structs

    /// <summary>
    ///     The epoll_data_t union is used to store the user data associated with a file descriptor.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct epoll_data_t
    {
        [FieldOffset(0)] public void* ptr;
        [FieldOffset(0)] public int fd;
        [FieldOffset(0)] public uint u32;
        [FieldOffset(0)] public ulong u64;
    }

    /// <summary>
    ///     The epoll_event structure describes an event that can be monitored for one file descriptor.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct epoll_event
    {
        /// <summary>
        ///     Epoll events
        /// </summary>
        public uint events;

        /// <summary>
        ///     User data variable
        /// </summary>
        public epoll_data_t data;
    }

    #endregion

    #region Constants

    /// <summary>
    ///     Set the close-on-exec (FD_CLOEXEC) flag on the new file descriptor.
    /// </summary>
    public const int EPOLL_CLOEXEC = 0x80000;

    /// <summary>
    ///     Add an entry to the interest list of the epoll file descriptor, epfd.
    ///     The entry includes the file descriptor, fd, a reference to the corresponding open file description,
    ///     and the settings specified in event.
    /// </summary>
    public const int EPOLL_CTL_ADD = 1;

    /// <summary>
    ///     Remove (deregister) the target file descriptor fd from the interest list.
    ///     The event argument is ignored and can be NULL(0).
    /// </summary>
    public const int EPOLL_CTL_DEL = 2;

    /// <summary>
    ///     Change the settings associated with fd in the interest list to the new settings specified in event.
    /// </summary>
    public const int EPOLL_CTL_MOD = 3;

    /// <summary>
    ///     The associated file is available for read(2) operations.
    /// </summary>
    public const int EPOLLIN = 0x001;

    /// <summary>
    ///     There is an exceptional condition on the file descriptor.
    /// </summary>
    public const int EPOLLPRI = 0x002;

    /// <summary>
    ///     The associated file is available for write(2) operations.
    /// </summary>
    public const int EPOLLOUT = 0x004;

    /// <summary>
    /// </summary>
    public const int EPOLLRDNORM = 0x040;

    public const int EPOLLRDBAND = 0x080;
    public const int EPOLLWRNORM = 0x100;
    public const int EPOLLWRBAND = 0x200;
    public const int EPOLLMSG = 0x400;

    /// <summary>
    ///     Error condition happened on the associated file descriptor.
    ///     This event is also reported for the write end of a pipe when the read end has been closed.
    ///     <remarks>
    ///         <see cref="epoll_wait" /> will always report for this event; it is not
    ///         necessary to set it in events when calling <see cref="epoll_ctl" />.
    ///     </remarks>
    /// </summary>
    public const int EPOLLERR = 0x008;

    /// <summary>
    ///     Hang up happened on the associated file descriptor.
    /// </summary>
    public const int EPOLLHUP = 0x010;

    /// <summary>
    ///     Stream socket peer closed connection, or shut down writing
    ///     half of connection.  (This flag is especially useful for
    ///     writing simple code to detect peer shutdown when using
    ///     edge-triggered monitoring.)
    /// </summary>
    public const int EPOLLRDHUP = 0x2000;

    /// <summary>
    ///     Sets an exclusive wake up mode for the epoll file descriptor that is being attached to the
    ///     associated file descriptor, the target file descriptor.
    ///     When a wake up event occurs and multiple epoll file descriptors are attached to the same
    ///     target file using this mode, one or more of the epoll file descriptors will receive an event
    ///     with <see cref="epoll_wait" />.
    /// </summary>
    public const int EPOLLEXCLUSIVE = 1 << 28;

    /// <summary>
    ///     If EPOLLONESHOT and EPOLLET are clear and the process has the CAP_BLOCK_SUSPEND capability,
    ///     ensure that the system does not enter "suspend" or "hibernate" while this event is pending or being processed.
    ///     The event is considered as being processed from the time when it is returned by a call to epoll_wait() until the
    ///     next call to epoll_wait() on the same epoll file descriptor, the closure of that file descriptor, the removal of
    ///     the event
    ///     file descriptor with EPOLL_CTL_DEL, or the clearing of EPOLLWAKEUP for the event file descriptor with
    ///     EPOLL_CTL_MOD.
    ///     <remarks>
    ///         This flag has no effect if EPOLLONESHOT or EPOLLET are set.
    ///     </remarks>
    /// </summary>
    public const int EPOLLWAKEUP = 1 << 29;

    /// <summary>
    ///     Requests one-shot notification for the associated file descriptor.
    ///     This means that after an event is pulled out
    ///     with epoll_wait(2) the associated file descriptor is internally disabled and no other events will be reported by
    ///     the epoll interface.
    ///     The user must call epoll_ctl() with EPOLL_CTL_MOD to rearm the file descriptor with a new
    ///     event mask.
    /// </summary>
    public const int EPOLLONESHOT = 1 << 30;

    /// <summary>
    ///     Requests edge-triggered notification for the associated file descriptor.
    ///     The default behavior for epoll is level-triggered.
    /// </summary>
    public const int EPOLLET = 1 << 31;

    #endregion
}