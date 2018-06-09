// C++/WinRT v1.0.170906.1
// Copyright (c) 2017 Microsoft Corporation. All rights reserved.

#pragma once
#include "winrt/impl/Windows.Foundation.1.h"
#include "winrt/impl/Windows.System.1.h"
#include "winrt/impl/Windows.ApplicationModel.Calls.1.h"

WINRT_EXPORT namespace winrt::Windows::ApplicationModel::Calls {

}

namespace winrt::impl {

}

WINRT_EXPORT namespace winrt::Windows::ApplicationModel::Calls {

struct WINRT_EBO CallAnswerEventArgs :
    Windows::ApplicationModel::Calls::ICallAnswerEventArgs
{
    CallAnswerEventArgs(std::nullptr_t) noexcept {}
};

struct WINRT_EBO CallRejectEventArgs :
    Windows::ApplicationModel::Calls::ICallRejectEventArgs
{
    CallRejectEventArgs(std::nullptr_t) noexcept {}
};

struct WINRT_EBO CallStateChangeEventArgs :
    Windows::ApplicationModel::Calls::ICallStateChangeEventArgs
{
    CallStateChangeEventArgs(std::nullptr_t) noexcept {}
};

struct WINRT_EBO LockScreenCallEndCallDeferral :
    Windows::ApplicationModel::Calls::ILockScreenCallEndCallDeferral
{
    LockScreenCallEndCallDeferral(std::nullptr_t) noexcept {}
};

struct WINRT_EBO LockScreenCallEndRequestedEventArgs :
    Windows::ApplicationModel::Calls::ILockScreenCallEndRequestedEventArgs
{
    LockScreenCallEndRequestedEventArgs(std::nullptr_t) noexcept {}
};

struct WINRT_EBO LockScreenCallUI :
    Windows::ApplicationModel::Calls::ILockScreenCallUI
{
    LockScreenCallUI(std::nullptr_t) noexcept {}
};

struct WINRT_EBO MuteChangeEventArgs :
    Windows::ApplicationModel::Calls::IMuteChangeEventArgs
{
    MuteChangeEventArgs(std::nullptr_t) noexcept {}
};

struct WINRT_EBO PhoneCallHistoryEntry :
    Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry
{
    PhoneCallHistoryEntry(std::nullptr_t) noexcept {}
    PhoneCallHistoryEntry();
};

struct WINRT_EBO PhoneCallHistoryEntryAddress :
    Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddress
{
    PhoneCallHistoryEntryAddress(std::nullptr_t) noexcept {}
    PhoneCallHistoryEntryAddress();
    PhoneCallHistoryEntryAddress(param::hstring const& rawAddress, Windows::ApplicationModel::Calls::PhoneCallHistoryEntryRawAddressKind const& rawAddressKind);
};

struct WINRT_EBO PhoneCallHistoryEntryQueryOptions :
    Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryQueryOptions
{
    PhoneCallHistoryEntryQueryOptions(std::nullptr_t) noexcept {}
    PhoneCallHistoryEntryQueryOptions();
};

struct WINRT_EBO PhoneCallHistoryEntryReader :
    Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryReader
{
    PhoneCallHistoryEntryReader(std::nullptr_t) noexcept {}
};

struct PhoneCallHistoryManager
{
    PhoneCallHistoryManager() = delete;
    static Windows::Foundation::IAsyncOperation<Windows::ApplicationModel::Calls::PhoneCallHistoryStore> RequestStoreAsync(Windows::ApplicationModel::Calls::PhoneCallHistoryStoreAccessType const& accessType);
    static Windows::ApplicationModel::Calls::PhoneCallHistoryManagerForUser GetForUser(Windows::System::User const& user);
};

struct WINRT_EBO PhoneCallHistoryManagerForUser :
    Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerForUser
{
    PhoneCallHistoryManagerForUser(std::nullptr_t) noexcept {}
};

struct WINRT_EBO PhoneCallHistoryStore :
    Windows::ApplicationModel::Calls::IPhoneCallHistoryStore
{
    PhoneCallHistoryStore(std::nullptr_t) noexcept {}
};

struct WINRT_EBO VoipCallCoordinator :
    Windows::ApplicationModel::Calls::IVoipCallCoordinator,
    impl::require<VoipCallCoordinator, Windows::ApplicationModel::Calls::IVoipCallCoordinator2>
{
    VoipCallCoordinator(std::nullptr_t) noexcept {}
    static Windows::ApplicationModel::Calls::VoipCallCoordinator GetDefault();
};

struct WINRT_EBO VoipPhoneCall :
    Windows::ApplicationModel::Calls::IVoipPhoneCall,
    impl::require<VoipPhoneCall, Windows::ApplicationModel::Calls::IVoipPhoneCall2>
{
    VoipPhoneCall(std::nullptr_t) noexcept {}
};

}
