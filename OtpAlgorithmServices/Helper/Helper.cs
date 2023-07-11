namespace OtpAlgorithmServices.Helper;

public static class Helper
{
    public static bool ByteArraysEqual(byte[] a, byte[] b)
    {
        if (ReferenceEquals(a, b))
        {
            return true;
        }

        if (a == null || b == null || a.Length != b.Length)
        {
            return false;
        }

        var areSame = true;

        for (var i = 0; i < a.Length; i++)
        {
            areSame &= (a[i] == b[i]);
        }

        return areSame;
    }
}