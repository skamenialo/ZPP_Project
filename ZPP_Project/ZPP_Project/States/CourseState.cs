
namespace ZPP_Project.States
{
    /// <summary>
    ///0000. state == created -&gt; !opened &amp;&amp; !stared &amp;&amp; !closed &amp;&amp; can(Start)<para />
    ///0001. state == opened -&gt; created &amp;&amp; pending &amp;&amp; !closed &amp;&amp; can(Close)<para />
    ///0010. state == started -&gt; created &amp;&amp; opened &amp;&amp; !closed &amp;&amp; can(Close)<para />
    ///0100. state == closed -&gt; created &amp;&amp; !opened &amp;&amp; !started &amp;&amp; !can(Start)<para />
    ///0101. state == closed | opened -&gt; created &amp;&amp; !started &amp;&amp; can(Start) &lt;- Cancelled<para />
    ///0110. state == closed | started -&gt; created &amp;&amp; opened &amp;&amp; can(Start) &lt;- Interrupted<para />
    /// </summary>
    public enum CourseState : byte
    {
        /// <summary>
        /// !opened &amp;&amp; !stared &amp;&amp; !closed &amp;&amp; can(Start)
        /// </summary>
        Created = 1,
        /// <summary>
        /// created &amp;&amp; pending &amp;&amp; !closed &amp;&amp; can(Close)
        /// </summary>
        Opened = 2,
        /// <summary>
        /// created &amp;&amp; opened &amp;&amp; !closed &amp;&amp; can(Close)
        /// </summary>
        Started = 3,
        /// <summary>
        /// created &amp;&amp; !opened &amp;&amp; !started &amp;&amp; !can(Start)
        /// </summary>
        Closed = 4
    }
}