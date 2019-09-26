// Win32ControlSampler.cpp : Defines the entry point for the application.
//

#include "framework.h"
#include "Win32ControlSampler.h"
#include "commctrl.h"
#include "richedit.h"
#include <initguid.h>
#include "objbase.h"
#include "uiautomation.h"

IAccPropServices* PropServices = nullptr;

void InitAccPropServices()
{
    HRESULT hr = CoCreateInstance(
        CLSID_AccPropServices,
        nullptr,
        CLSCTX_INPROC,
        IID_PPV_ARGS(&PropServices));
}

void SetAccessibleName(HWND hWnd, LPCWSTR name)
{
    PropServices->SetHwndPropStr(
        hWnd,
        OBJID_CLIENT,
        CHILDID_SELF,
        Name_Property_GUID,
        name);
}

void InsertTabItem(HWND hWnd, int index, LPWSTR text)
{
    TCITEM tci = { 0 };
    tci.mask = TCIF_TEXT;
    tci.pszText = text;
    tci.cchTextMax = wcslen(text);
    SendMessage(hWnd, TCM_INSERTITEM, index, (LPARAM)&tci);
}

void InitDialog(HWND hDlg)
{
    const LPCWSTR Item1 = L"First Item";
    const LPCWSTR Item2 = L"Second Item";
    const LPCWSTR LongText = L"Video provides a powerful way to help you prove your point. When you click Online Video, you can paste in the embed code for the video you want to add. You can also type a keyword to search online for the video that best fits your document.";

    HWND hwndCombo = GetDlgItem(hDlg, IDC_COMBO1);
    SendMessage(hwndCombo, CB_ADDSTRING, 0, (LPARAM)Item1);
    SendMessage(hwndCombo, CB_ADDSTRING, 0, (LPARAM)Item2);
    SetAccessibleName(hwndCombo, L"Some flexible choice");

    HWND hwndList = GetDlgItem(hDlg, IDC_LIST1);
    SendMessage(hwndList, LB_ADDSTRING, 0, (LPARAM)Item1);
    SendMessage(hwndList, LB_ADDSTRING, 0, (LPARAM)Item2);
    SetAccessibleName(hwndList, L"Some fixed choice");

    HWND hwndTab = GetDlgItem(hDlg, IDC_TAB1);
    InsertTabItem(hwndTab, 0, (LPWSTR)L"Tab 1");
    InsertTabItem(hwndTab, 1, (LPWSTR)L"Tab 2");

    CHARRANGE cr;
    cr.cpMin = -1;
    cr.cpMax = -1;

    HWND hwndRichEdit = GetDlgItem(hDlg, IDC_RICHEDIT21);
    SendMessage(hwndRichEdit, EM_EXSETSEL, 0, (LPARAM)&cr);
    SendMessage(hwndRichEdit, EM_REPLACESEL, 0, (LPARAM)LongText);
    SetAccessibleName(hwndRichEdit, L"A message");

    SetAccessibleName(GetDlgItem(hDlg, IDC_SLIDER1), L"Pick a number!");
}

INT_PTR CALLBACK DialogProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    UNREFERENCED_PARAMETER(lParam);
    switch (message)
    {
    case WM_INITDIALOG:
        InitDialog(hDlg);
        return (INT_PTR)TRUE;

    case WM_COMMAND:
        if (LOWORD(wParam) == IDCANCEL)
        {
            EndDialog(hDlg, LOWORD(wParam));
            return (INT_PTR)TRUE;
        }
        break;
    }
    return (INT_PTR)FALSE;
}

int APIENTRY wWinMain(_In_ HINSTANCE hInstance,
                     _In_opt_ HINSTANCE hPrevInstance,
                     _In_ LPWSTR    lpCmdLine,
                     _In_ int       nCmdShow)
{
    HRESULT hr = CoInitializeEx(nullptr, COINIT::COINIT_SPEED_OVER_MEMORY);

    InitAccPropServices();
    LoadLibraryW(L"Riched32.dll");  // Necessary for the Rich Edit control
    DialogBox(hInstance, MAKEINTRESOURCE(IDD_SAMPLER), nullptr, DialogProc);
    if (SUCCEEDED(hr))
        CoUninitialize();

    return 0;
}

