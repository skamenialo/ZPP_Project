
namespace ZPP_Project.States
{
    /// <summary>
    ///0000. state == pending -&gt; !started &amp;&amp; !finished &amp;&amp; !checked &amp;&amp; can(Begin)<para/>
    ///0001. state == started -&gt; !pending &amp;&amp; !finished &amp;&amp; !checked &amp;&amp; (can(CheckPresence) || can(Finish) || (can(CheckPresence) &amp;&amp; can(Finish)))<para/>
    ///0010. state == checked -&gt; !pending &amp;&amp; !can(Finish)// cannot use alone<para/>
    ///0100. state == finished -&gt; !pending &amp;&amp; !started &amp;&amp; !checked &amp;&amp; !can(Begin) &amp;&amp; can(CheckPresence)<para/>
    ///0011. state == started | checked -&gt; !pending &amp;&amp; !finished &amp;&amp; can(Finish)<para/>
    ///0110. state == finished | checked -&gt; !pending &amp;&amp; !started &amp;&amp; !can(Begin) &amp;&amp; !can(CheckPresence)<para/>
    /// </summary>
    public enum LectureState : byte
    {
        /// <summary>
        /// !started &amp;&amp; !finished &amp;&amp; !checked &amp;&amp; can(Begin)
        /// </summary>
        pending = 0,
        /// <summary>
        /// !pending &amp;&amp; !finished &amp;&amp; !checked &amp;&amp; (can(CheckPresence) || can(Finish) || (can(CheckPresence) &amp;&amp; can(Finish)))
        /// </summary>
        started = 1,
        /// <summary>
        /// !pending &amp;&amp; !can(Finish)// cannot use alone
        /// </summary>
        @checked = 2,
        /// <summary>
        /// !pending &amp;&amp; !started &amp;&amp; !checked &amp;&amp; !can(Begin) &amp;&amp; can(CheckPresence)
        /// </summary>
        finished = 4
    }
}