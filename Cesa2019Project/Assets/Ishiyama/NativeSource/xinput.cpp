#include "xinput.h"
#include <Windows.h>
#include <Xinput.h>
#include <cstdint>
#include <algorithm>
#pragma comment(lib,"xinput.lib")

void Vibration(int n, float l_power, float r_power)
{
	XINPUT_VIBRATION vibration;
	vibration.wRightMotorSpeed = static_cast<WORD>(UINT16_MAX * std::clamp(r_power, 0.0f, 1.0f));
	vibration.wLeftMotorSpeed = static_cast<WORD>(UINT16_MAX * std::clamp(l_power, 0.0f, 1.0f));
	XInputSetState(static_cast<DWORD>(n), &vibration);
}

float GetTrigger(int n, bool is_right)
{
	XINPUT_STATE xinput_state;
	//xinputのステートを取得
	auto dw_result = XInputGetState(static_cast<DWORD>(n), &xinput_state);
	//取得に失敗
	if (dw_result != ERROR_SUCCESS)return 0.0f;
	return ((is_right) ? (xinput_state.Gamepad.bRightTrigger) : (xinput_state.Gamepad.bLeftTrigger)) / 255.0f;
}