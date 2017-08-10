
namespace ZPP_Project.States
{
    /// <summary>
    /// 0000 - state == signed -&gt; !accepted &amp;&amp; !rejected &amp;&amp; !done &amp;&amp; can(AcceptReject) &amp;&amp; can(SignOut) &amp;&amp; !can(SignIn)<para />
    /// 0001 - state == accepted -&gt; signed &amp;&amp; !rejected &amp;&amp; !done &amp;&amp; can(SignOut) &amp;&amp; !can(SignIn)<para />
    /// 0010 - state == rejected -&gt; signed &amp;&amp; !accepted &amp;&amp; !done &amp;&amp; ForceSignOut &amp;&amp; !can(SignIn) &amp;&amp; !can(SignInInFuture)<para />
    /// 0100 - state == done -&gt; signed &amp;&amp; !can(SignOut) &amp;&amp; !can(SignIn) // cannot use alone<para />
    /// 0101 - state == done | accepted -&gt; signed &amp;&amp; accepted &amp;&amp; !can(SignOut) &amp;&amp; !can(SignIn) &lt;- pass<para />
    /// 0111 - state == done | accepted | rejected -&gt; signed &amp;&amp; accepted &amp;&amp; rejected &amp;&amp; !can(SignOut) &amp;&amp; !can(SignIn) &lt;- give up<para />
    /// </summary>
    public enum StudentState : byte
    {
        /// <summary>
        /// !accepted &amp;&amp; !rejected &amp;&amp; !done &amp;&amp; can(AcceptReject) &amp;&amp; can(SignOut) &amp;&amp; !can(SignIn)
        /// </summary>
        signed = 0,
        /// <summary>
        /// signed &amp;&amp; !rejected &amp;&amp; !done &amp;&amp; can(SignOut) &amp;&amp; !can(SignIn)
        /// </summary>
        accepted = 1,
        /// <summary>
        /// signed &amp;&amp; !accepted &amp;&amp; !done &amp;&amp; ForceSignOut &amp;&amp; !can(SignIn) &amp;&amp; !can(SignInInFuture)
        /// </summary>
        rejected = 2,
        /// <summary>
        /// signed &amp;&amp; !can(SignOut) &amp;&amp; !can(SignIn) // cannot use alone
        /// </summary>
        done = 4
    }
}