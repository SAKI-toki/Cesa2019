using System.Runtime.InteropServices;

public class VibrationController
{
    [DllImport("VibrationDll")]
    static extern void Vibration(int n, float l_power, float r_power);
    
    static public void XVibration(int n, float l_power, float r_power)
    {
        Vibration(n, l_power, r_power);
    }
}
