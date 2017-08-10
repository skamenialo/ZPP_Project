
namespace ZPP_Project.States
{
    /// <summary>
    ///0000. state == anonymous -&gt; loggedOut &amp;&amp; !loggedIn &amp;&amp; !blocked &amp;&amp; can(LogIn)<para/>
    ///0001. state == inactive -&gt; loggedOut &amp;&amp; !active &amp;&amp; !loggedIn &amp;&amp; !blocked &amp;&amp; !can(LogIn)<para/>
    ///0010. state == active -&gt; !inactive &amp;&amp; !blocked &amp;&amp; can(LogIn)<para/>
    ///0100. state == loggedIn -&gt; !loggedOut &amp;&amp; !anonymous &amp;&amp; !can(LogOut) // cannot use alone<para/>
    ///1000. state == blocked -&gt; !can(logIn)<para/>
    ///0101. state == inactive &amp;&amp; loggedIn -&gt; forceLogOut()<para/>
    ///0110. state == active &amp;&amp; loggedIn -&gt; !blocked &amp;&amp; can(LogOut)<para/>
    ///1001. state == blocked | inactive -&gt; !can(logIn) &amp;&amp; !can(activate)<para/>
    ///1010. state == blocked | active -&gt; !can(logIn)<para/>
    ///1110. state == loggedIn | blocked -&gt; forceLogOut() &amp;&amp; showMessage()<para/>
    /// </summary>
    public enum UserState : byte
    {
        /// <summary>
        /// loggedOut &amp;&amp; !loggedIn &amp;&amp; !blocked &amp;&amp; can(LogIn)
        /// </summary>
        anonymous = 0,
        /// <summary>
        /// loggedOut &amp;&amp; !active &amp;&amp; !loggedIn &amp;&amp; !blocked &amp;&amp; !can(LogIn)
        /// </summary>
        inactive = 1,
        /// <summary>
        /// !inactive &amp;&amp; !blocked &amp;&amp; can(LogIn)
        /// </summary>
        active = 2,
        /// <summary>
        /// !loggedOut &amp;&amp; !anonymous &amp;&amp; !can(LogOut) // cannot use alone
        /// </summary>
        loggedIn = 4,
        /// <summary>
        /// !can(logIn)
        /// </summary>
        blocked = 8
    }
}