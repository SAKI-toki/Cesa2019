#include "main.h"
#include <Windows.h>
#include <Xinput.h>
#include <cstdint>
#pragma comment(lib,"xinput.lib")

template<typename T>
T Clamp(T n, T min_n, T max_n)
{
	if (n < min_n)return min_n;
	if (n > max_n)return max_n;
	return n;
}

void Vibration(int n, float l_power, float r_power)
{
	XINPUT_VIBRATION vibration;
	vibration.wRightMotorSpeed = static_cast<WORD>(UINT16_MAX * Clamp(r_power, 0.0f, 1.0f));
	vibration.wLeftMotorSpeed = static_cast<WORD>(UINT16_MAX * Clamp(l_power, 0.0f, 1.0f));
	XInputSetState(static_cast<DWORD>(n), &vibration);
}