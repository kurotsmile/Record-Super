// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("miiriJqnrKOALOIsXaerq6uvqql+18DZw4b93EgVAxqKLHyovQ8r5r2Cf29Sv2lyymVflTHovIidL2LEKKulqpooq6CoKKurqm83udtJUQcTtD3KagldH9GK8L2ZViHmX6NBJ6npN2Kd6YB1q3f0nh99pLPmwv/hYoIqs5uChAqDx0C3gdZJzVJ1hCrxsqsM4QhPUZrljmXODBvLt0rRz9EpRT0CPc01OHRuwVjqrAwTX/hTskoCjzdBl/Y+w9fkEkq6SVLbSYSVvfWrjMNhCtTeI3CDUNLJjofWOgTohPzzkfmLHr8WJdS7N/YV4UpR6OWP/Zvu+5t7ZMbS692/eQB0dgoDPwFApIdwpUnnL4eUgjo4O8U5PxASQc33Kj1nJaipq6qr");
        private static int[] order = new int[] { 0,7,10,7,5,6,8,13,8,9,10,11,13,13,14 };
        private static int key = 170;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
