// C++/WinRT v1.0.170906.1
// Copyright (c) 2017 Microsoft Corporation. All rights reserved.

#pragma once
#include "winrt/base.h"

WINRT_WARNING_PUSH
#include "winrt/Windows.Foundation.h"
#include "winrt/Windows.Foundation.Collections.h"
#include "winrt/impl/Windows.Foundation.2.h"
#include "winrt/impl/Windows.System.2.h"
#include "winrt/impl/Windows.ApplicationModel.Calls.2.h"
#include "winrt/Windows.ApplicationModel.h"

namespace winrt::impl {

template <typename D> Windows::ApplicationModel::Calls::VoipPhoneCallMedia consume_Windows_ApplicationModel_Calls_ICallAnswerEventArgs<D>::AcceptedMedia() const noexcept
{
    Windows::ApplicationModel::Calls::VoipPhoneCallMedia value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::ICallAnswerEventArgs)->get_AcceptedMedia(put_abi(value)));
    return value;
}

template <typename D> Windows::ApplicationModel::Calls::VoipPhoneCallRejectReason consume_Windows_ApplicationModel_Calls_ICallRejectEventArgs<D>::RejectReason() const noexcept
{
    Windows::ApplicationModel::Calls::VoipPhoneCallRejectReason value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::ICallRejectEventArgs)->get_RejectReason(put_abi(value)));
    return value;
}

template <typename D> Windows::ApplicationModel::Calls::VoipPhoneCallState consume_Windows_ApplicationModel_Calls_ICallStateChangeEventArgs<D>::State() const noexcept
{
    Windows::ApplicationModel::Calls::VoipPhoneCallState value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::ICallStateChangeEventArgs)->get_State(put_abi(value)));
    return value;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_ILockScreenCallEndCallDeferral<D>::Complete() const
{
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::ILockScreenCallEndCallDeferral)->Complete());
}

template <typename D> Windows::ApplicationModel::Calls::LockScreenCallEndCallDeferral consume_Windows_ApplicationModel_Calls_ILockScreenCallEndRequestedEventArgs<D>::GetDeferral() const
{
    Windows::ApplicationModel::Calls::LockScreenCallEndCallDeferral value{ nullptr };
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::ILockScreenCallEndRequestedEventArgs)->GetDeferral(put_abi(value)));
    return value;
}

template <typename D> Windows::Foundation::DateTime consume_Windows_ApplicationModel_Calls_ILockScreenCallEndRequestedEventArgs<D>::Deadline() const noexcept
{
    Windows::Foundation::DateTime value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::ILockScreenCallEndRequestedEventArgs)->get_Deadline(put_abi(value)));
    return value;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_ILockScreenCallUI<D>::Dismiss() const
{
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::ILockScreenCallUI)->Dismiss());
}

template <typename D> event_token consume_Windows_ApplicationModel_Calls_ILockScreenCallUI<D>::EndRequested(Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::LockScreenCallUI, Windows::ApplicationModel::Calls::LockScreenCallEndRequestedEventArgs> const& handler) const
{
    event_token token{};
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::ILockScreenCallUI)->add_EndRequested(get_abi(handler), put_abi(token)));
    return token;
}

template <typename D> event_revoker<Windows::ApplicationModel::Calls::ILockScreenCallUI> consume_Windows_ApplicationModel_Calls_ILockScreenCallUI<D>::EndRequested(auto_revoke_t, Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::LockScreenCallUI, Windows::ApplicationModel::Calls::LockScreenCallEndRequestedEventArgs> const& handler) const
{
    return impl::make_event_revoker<D, Windows::ApplicationModel::Calls::ILockScreenCallUI>(this, &abi_t<Windows::ApplicationModel::Calls::ILockScreenCallUI>::remove_EndRequested, EndRequested(handler));
}

template <typename D> void consume_Windows_ApplicationModel_Calls_ILockScreenCallUI<D>::EndRequested(event_token const& token) const
{
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::ILockScreenCallUI)->remove_EndRequested(get_abi(token)));
}

template <typename D> event_token consume_Windows_ApplicationModel_Calls_ILockScreenCallUI<D>::Closed(Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::LockScreenCallUI, Windows::Foundation::IInspectable> const& handler) const
{
    event_token token{};
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::ILockScreenCallUI)->add_Closed(get_abi(handler), put_abi(token)));
    return token;
}

template <typename D> event_revoker<Windows::ApplicationModel::Calls::ILockScreenCallUI> consume_Windows_ApplicationModel_Calls_ILockScreenCallUI<D>::Closed(auto_revoke_t, Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::LockScreenCallUI, Windows::Foundation::IInspectable> const& handler) const
{
    return impl::make_event_revoker<D, Windows::ApplicationModel::Calls::ILockScreenCallUI>(this, &abi_t<Windows::ApplicationModel::Calls::ILockScreenCallUI>::remove_Closed, Closed(handler));
}

template <typename D> void consume_Windows_ApplicationModel_Calls_ILockScreenCallUI<D>::Closed(event_token const& token) const
{
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::ILockScreenCallUI)->remove_Closed(get_abi(token)));
}

template <typename D> hstring consume_Windows_ApplicationModel_Calls_ILockScreenCallUI<D>::CallTitle() const noexcept
{
    hstring value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::ILockScreenCallUI)->get_CallTitle(put_abi(value)));
    return value;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_ILockScreenCallUI<D>::CallTitle(param::hstring const& value) const noexcept
{
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::ILockScreenCallUI)->put_CallTitle(get_abi(value)));
}

template <typename D> bool consume_Windows_ApplicationModel_Calls_IMuteChangeEventArgs<D>::Muted() const noexcept
{
    bool value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IMuteChangeEventArgs)->get_Muted(&value));
    return value;
}

template <typename D> hstring consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::Id() const noexcept
{
    hstring value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->get_Id(put_abi(value)));
    return value;
}

template <typename D> Windows::ApplicationModel::Calls::PhoneCallHistoryEntryAddress consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::Address() const noexcept
{
    Windows::ApplicationModel::Calls::PhoneCallHistoryEntryAddress value{ nullptr };
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->get_Address(put_abi(value)));
    return value;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::Address(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryAddress const& value) const noexcept
{
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->put_Address(get_abi(value)));
}

template <typename D> Windows::Foundation::IReference<Windows::Foundation::TimeSpan> consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::Duration() const noexcept
{
    Windows::Foundation::IReference<Windows::Foundation::TimeSpan> value{ nullptr };
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->get_Duration(put_abi(value)));
    return value;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::Duration(optional<Windows::Foundation::TimeSpan> const& value) const noexcept
{
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->put_Duration(get_abi(value)));
}

template <typename D> bool consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::IsCallerIdBlocked() const noexcept
{
    bool value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->get_IsCallerIdBlocked(&value));
    return value;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::IsCallerIdBlocked(bool value) const noexcept
{
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->put_IsCallerIdBlocked(value));
}

template <typename D> bool consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::IsEmergency() const noexcept
{
    bool value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->get_IsEmergency(&value));
    return value;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::IsEmergency(bool value) const noexcept
{
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->put_IsEmergency(value));
}

template <typename D> bool consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::IsIncoming() const noexcept
{
    bool value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->get_IsIncoming(&value));
    return value;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::IsIncoming(bool value) const noexcept
{
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->put_IsIncoming(value));
}

template <typename D> bool consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::IsMissed() const noexcept
{
    bool value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->get_IsMissed(&value));
    return value;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::IsMissed(bool value) const noexcept
{
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->put_IsMissed(value));
}

template <typename D> bool consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::IsRinging() const noexcept
{
    bool value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->get_IsRinging(&value));
    return value;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::IsRinging(bool value) const noexcept
{
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->put_IsRinging(value));
}

template <typename D> bool consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::IsSeen() const noexcept
{
    bool value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->get_IsSeen(&value));
    return value;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::IsSeen(bool value) const noexcept
{
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->put_IsSeen(value));
}

template <typename D> bool consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::IsSuppressed() const noexcept
{
    bool value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->get_IsSuppressed(&value));
    return value;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::IsSuppressed(bool value) const noexcept
{
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->put_IsSuppressed(value));
}

template <typename D> bool consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::IsVoicemail() const noexcept
{
    bool value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->get_IsVoicemail(&value));
    return value;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::IsVoicemail(bool value) const noexcept
{
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->put_IsVoicemail(value));
}

template <typename D> Windows::ApplicationModel::Calls::PhoneCallHistoryEntryMedia consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::Media() const noexcept
{
    Windows::ApplicationModel::Calls::PhoneCallHistoryEntryMedia value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->get_Media(put_abi(value)));
    return value;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::Media(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryMedia const& value) const noexcept
{
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->put_Media(get_abi(value)));
}

template <typename D> Windows::ApplicationModel::Calls::PhoneCallHistoryEntryOtherAppReadAccess consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::OtherAppReadAccess() const noexcept
{
    Windows::ApplicationModel::Calls::PhoneCallHistoryEntryOtherAppReadAccess value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->get_OtherAppReadAccess(put_abi(value)));
    return value;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::OtherAppReadAccess(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryOtherAppReadAccess const& value) const noexcept
{
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->put_OtherAppReadAccess(get_abi(value)));
}

template <typename D> hstring consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::RemoteId() const noexcept
{
    hstring value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->get_RemoteId(put_abi(value)));
    return value;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::RemoteId(param::hstring const& value) const noexcept
{
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->put_RemoteId(get_abi(value)));
}

template <typename D> hstring consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::SourceDisplayName() const noexcept
{
    hstring value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->get_SourceDisplayName(put_abi(value)));
    return value;
}

template <typename D> hstring consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::SourceId() const noexcept
{
    hstring value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->get_SourceId(put_abi(value)));
    return value;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::SourceId(param::hstring const& value) const noexcept
{
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->put_SourceId(get_abi(value)));
}

template <typename D> Windows::ApplicationModel::Calls::PhoneCallHistorySourceIdKind consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::SourceIdKind() const noexcept
{
    Windows::ApplicationModel::Calls::PhoneCallHistorySourceIdKind value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->get_SourceIdKind(put_abi(value)));
    return value;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::SourceIdKind(Windows::ApplicationModel::Calls::PhoneCallHistorySourceIdKind const& value) const noexcept
{
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->put_SourceIdKind(get_abi(value)));
}

template <typename D> Windows::Foundation::DateTime consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::StartTime() const noexcept
{
    Windows::Foundation::DateTime value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->get_StartTime(put_abi(value)));
    return value;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>::StartTime(Windows::Foundation::DateTime const& value) const noexcept
{
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry)->put_StartTime(get_abi(value)));
}

template <typename D> hstring consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntryAddress<D>::ContactId() const noexcept
{
    hstring value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddress)->get_ContactId(put_abi(value)));
    return value;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntryAddress<D>::ContactId(param::hstring const& value) const noexcept
{
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddress)->put_ContactId(get_abi(value)));
}

template <typename D> hstring consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntryAddress<D>::DisplayName() const noexcept
{
    hstring value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddress)->get_DisplayName(put_abi(value)));
    return value;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntryAddress<D>::DisplayName(param::hstring const& value) const noexcept
{
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddress)->put_DisplayName(get_abi(value)));
}

template <typename D> hstring consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntryAddress<D>::RawAddress() const noexcept
{
    hstring value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddress)->get_RawAddress(put_abi(value)));
    return value;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntryAddress<D>::RawAddress(param::hstring const& value) const noexcept
{
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddress)->put_RawAddress(get_abi(value)));
}

template <typename D> Windows::ApplicationModel::Calls::PhoneCallHistoryEntryRawAddressKind consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntryAddress<D>::RawAddressKind() const noexcept
{
    Windows::ApplicationModel::Calls::PhoneCallHistoryEntryRawAddressKind value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddress)->get_RawAddressKind(put_abi(value)));
    return value;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntryAddress<D>::RawAddressKind(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryRawAddressKind const& value) const noexcept
{
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddress)->put_RawAddressKind(get_abi(value)));
}

template <typename D> Windows::ApplicationModel::Calls::PhoneCallHistoryEntryAddress consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntryAddressFactory<D>::Create(param::hstring const& rawAddress, Windows::ApplicationModel::Calls::PhoneCallHistoryEntryRawAddressKind const& rawAddressKind) const
{
    Windows::ApplicationModel::Calls::PhoneCallHistoryEntryAddress result{ nullptr };
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddressFactory)->Create(get_abi(rawAddress), get_abi(rawAddressKind), put_abi(result)));
    return result;
}

template <typename D> Windows::ApplicationModel::Calls::PhoneCallHistoryEntryQueryDesiredMedia consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntryQueryOptions<D>::DesiredMedia() const noexcept
{
    Windows::ApplicationModel::Calls::PhoneCallHistoryEntryQueryDesiredMedia value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryQueryOptions)->get_DesiredMedia(put_abi(value)));
    return value;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntryQueryOptions<D>::DesiredMedia(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryQueryDesiredMedia const& value) const noexcept
{
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryQueryOptions)->put_DesiredMedia(get_abi(value)));
}

template <typename D> Windows::Foundation::Collections::IVector<hstring> consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntryQueryOptions<D>::SourceIds() const noexcept
{
    Windows::Foundation::Collections::IVector<hstring> value{ nullptr };
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryQueryOptions)->get_SourceIds(put_abi(value)));
    return value;
}

template <typename D> Windows::Foundation::IAsyncOperation<Windows::Foundation::Collections::IVectorView<Windows::ApplicationModel::Calls::PhoneCallHistoryEntry>> consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntryReader<D>::ReadBatchAsync() const
{
    Windows::Foundation::IAsyncOperation<Windows::Foundation::Collections::IVectorView<Windows::ApplicationModel::Calls::PhoneCallHistoryEntry>> result{ nullptr };
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryReader)->ReadBatchAsync(put_abi(result)));
    return result;
}

template <typename D> Windows::Foundation::IAsyncOperation<Windows::ApplicationModel::Calls::PhoneCallHistoryStore> consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryManagerForUser<D>::RequestStoreAsync(Windows::ApplicationModel::Calls::PhoneCallHistoryStoreAccessType const& accessType) const
{
    Windows::Foundation::IAsyncOperation<Windows::ApplicationModel::Calls::PhoneCallHistoryStore> result{ nullptr };
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerForUser)->RequestStoreAsync(get_abi(accessType), put_abi(result)));
    return result;
}

template <typename D> Windows::System::User consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryManagerForUser<D>::User() const noexcept
{
    Windows::System::User value{ nullptr };
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerForUser)->get_User(put_abi(value)));
    return value;
}

template <typename D> Windows::Foundation::IAsyncOperation<Windows::ApplicationModel::Calls::PhoneCallHistoryStore> consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryManagerStatics<D>::RequestStoreAsync(Windows::ApplicationModel::Calls::PhoneCallHistoryStoreAccessType const& accessType) const
{
    Windows::Foundation::IAsyncOperation<Windows::ApplicationModel::Calls::PhoneCallHistoryStore> result{ nullptr };
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerStatics)->RequestStoreAsync(get_abi(accessType), put_abi(result)));
    return result;
}

template <typename D> Windows::ApplicationModel::Calls::PhoneCallHistoryManagerForUser consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryManagerStatics2<D>::GetForUser(Windows::System::User const& user) const
{
    Windows::ApplicationModel::Calls::PhoneCallHistoryManagerForUser result{ nullptr };
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerStatics2)->GetForUser(get_abi(user), put_abi(result)));
    return result;
}

template <typename D> Windows::Foundation::IAsyncOperation<Windows::ApplicationModel::Calls::PhoneCallHistoryEntry> consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryStore<D>::GetEntryAsync(param::hstring const& callHistoryEntryId) const
{
    Windows::Foundation::IAsyncOperation<Windows::ApplicationModel::Calls::PhoneCallHistoryEntry> result{ nullptr };
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryStore)->GetEntryAsync(get_abi(callHistoryEntryId), put_abi(result)));
    return result;
}

template <typename D> Windows::ApplicationModel::Calls::PhoneCallHistoryEntryReader consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryStore<D>::GetEntryReader() const
{
    Windows::ApplicationModel::Calls::PhoneCallHistoryEntryReader result{ nullptr };
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryStore)->GetEntryReader(put_abi(result)));
    return result;
}

template <typename D> Windows::ApplicationModel::Calls::PhoneCallHistoryEntryReader consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryStore<D>::GetEntryReader(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryQueryOptions const& queryOptions) const
{
    Windows::ApplicationModel::Calls::PhoneCallHistoryEntryReader result{ nullptr };
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryStore)->GetEntryReaderWithOptions(get_abi(queryOptions), put_abi(result)));
    return result;
}

template <typename D> Windows::Foundation::IAsyncAction consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryStore<D>::SaveEntryAsync(Windows::ApplicationModel::Calls::PhoneCallHistoryEntry const& callHistoryEntry) const
{
    Windows::Foundation::IAsyncAction result{ nullptr };
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryStore)->SaveEntryAsync(get_abi(callHistoryEntry), put_abi(result)));
    return result;
}

template <typename D> Windows::Foundation::IAsyncAction consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryStore<D>::DeleteEntryAsync(Windows::ApplicationModel::Calls::PhoneCallHistoryEntry const& callHistoryEntry) const
{
    Windows::Foundation::IAsyncAction result{ nullptr };
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryStore)->DeleteEntryAsync(get_abi(callHistoryEntry), put_abi(result)));
    return result;
}

template <typename D> Windows::Foundation::IAsyncAction consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryStore<D>::DeleteEntriesAsync(param::async_iterable<Windows::ApplicationModel::Calls::PhoneCallHistoryEntry> const& callHistoryEntries) const
{
    Windows::Foundation::IAsyncAction result{ nullptr };
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryStore)->DeleteEntriesAsync(get_abi(callHistoryEntries), put_abi(result)));
    return result;
}

template <typename D> Windows::Foundation::IAsyncAction consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryStore<D>::MarkEntryAsSeenAsync(Windows::ApplicationModel::Calls::PhoneCallHistoryEntry const& callHistoryEntry) const
{
    Windows::Foundation::IAsyncAction result{ nullptr };
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryStore)->MarkEntryAsSeenAsync(get_abi(callHistoryEntry), put_abi(result)));
    return result;
}

template <typename D> Windows::Foundation::IAsyncAction consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryStore<D>::MarkEntriesAsSeenAsync(param::async_iterable<Windows::ApplicationModel::Calls::PhoneCallHistoryEntry> const& callHistoryEntries) const
{
    Windows::Foundation::IAsyncAction result{ nullptr };
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryStore)->MarkEntriesAsSeenAsync(get_abi(callHistoryEntries), put_abi(result)));
    return result;
}

template <typename D> Windows::Foundation::IAsyncOperation<uint32_t> consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryStore<D>::GetUnseenCountAsync() const
{
    Windows::Foundation::IAsyncOperation<uint32_t> result{ nullptr };
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryStore)->GetUnseenCountAsync(put_abi(result)));
    return result;
}

template <typename D> Windows::Foundation::IAsyncAction consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryStore<D>::MarkAllAsSeenAsync() const
{
    Windows::Foundation::IAsyncAction result{ nullptr };
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryStore)->MarkAllAsSeenAsync(put_abi(result)));
    return result;
}

template <typename D> Windows::Foundation::IAsyncOperation<uint32_t> consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryStore<D>::GetSourcesUnseenCountAsync(param::async_iterable<hstring> const& sourceIds) const
{
    Windows::Foundation::IAsyncOperation<uint32_t> result{ nullptr };
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryStore)->GetSourcesUnseenCountAsync(get_abi(sourceIds), put_abi(result)));
    return result;
}

template <typename D> Windows::Foundation::IAsyncAction consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryStore<D>::MarkSourcesAsSeenAsync(param::async_iterable<hstring> const& sourceIds) const
{
    Windows::Foundation::IAsyncAction result{ nullptr };
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IPhoneCallHistoryStore)->MarkSourcesAsSeenAsync(get_abi(sourceIds), put_abi(result)));
    return result;
}

template <typename D> Windows::Foundation::IAsyncOperation<Windows::ApplicationModel::Calls::VoipPhoneCallResourceReservationStatus> consume_Windows_ApplicationModel_Calls_IVoipCallCoordinator<D>::ReserveCallResourcesAsync(param::hstring const& taskEntryPoint) const
{
    Windows::Foundation::IAsyncOperation<Windows::ApplicationModel::Calls::VoipPhoneCallResourceReservationStatus> operation{ nullptr };
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipCallCoordinator)->ReserveCallResourcesAsync(get_abi(taskEntryPoint), put_abi(operation)));
    return operation;
}

template <typename D> event_token consume_Windows_ApplicationModel_Calls_IVoipCallCoordinator<D>::MuteStateChanged(Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipCallCoordinator, Windows::ApplicationModel::Calls::MuteChangeEventArgs> const& muteChangeHandler) const
{
    event_token token{};
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipCallCoordinator)->add_MuteStateChanged(get_abi(muteChangeHandler), put_abi(token)));
    return token;
}

template <typename D> event_revoker<Windows::ApplicationModel::Calls::IVoipCallCoordinator> consume_Windows_ApplicationModel_Calls_IVoipCallCoordinator<D>::MuteStateChanged(auto_revoke_t, Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipCallCoordinator, Windows::ApplicationModel::Calls::MuteChangeEventArgs> const& muteChangeHandler) const
{
    return impl::make_event_revoker<D, Windows::ApplicationModel::Calls::IVoipCallCoordinator>(this, &abi_t<Windows::ApplicationModel::Calls::IVoipCallCoordinator>::remove_MuteStateChanged, MuteStateChanged(muteChangeHandler));
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IVoipCallCoordinator<D>::MuteStateChanged(event_token const& token) const
{
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipCallCoordinator)->remove_MuteStateChanged(get_abi(token)));
}

template <typename D> Windows::ApplicationModel::Calls::VoipPhoneCall consume_Windows_ApplicationModel_Calls_IVoipCallCoordinator<D>::RequestNewIncomingCall(param::hstring const& context, param::hstring const& contactName, param::hstring const& contactNumber, Windows::Foundation::Uri const& contactImage, param::hstring const& serviceName, Windows::Foundation::Uri const& brandingImage, param::hstring const& callDetails, Windows::Foundation::Uri const& ringtone, Windows::ApplicationModel::Calls::VoipPhoneCallMedia const& media, Windows::Foundation::TimeSpan const& ringTimeout) const
{
    Windows::ApplicationModel::Calls::VoipPhoneCall call{ nullptr };
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipCallCoordinator)->RequestNewIncomingCall(get_abi(context), get_abi(contactName), get_abi(contactNumber), get_abi(contactImage), get_abi(serviceName), get_abi(brandingImage), get_abi(callDetails), get_abi(ringtone), get_abi(media), get_abi(ringTimeout), put_abi(call)));
    return call;
}

template <typename D> Windows::ApplicationModel::Calls::VoipPhoneCall consume_Windows_ApplicationModel_Calls_IVoipCallCoordinator<D>::RequestNewOutgoingCall(param::hstring const& context, param::hstring const& contactName, param::hstring const& serviceName, Windows::ApplicationModel::Calls::VoipPhoneCallMedia const& media) const
{
    Windows::ApplicationModel::Calls::VoipPhoneCall call{ nullptr };
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipCallCoordinator)->RequestNewOutgoingCall(get_abi(context), get_abi(contactName), get_abi(serviceName), get_abi(media), put_abi(call)));
    return call;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IVoipCallCoordinator<D>::NotifyMuted() const
{
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipCallCoordinator)->NotifyMuted());
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IVoipCallCoordinator<D>::NotifyUnmuted() const
{
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipCallCoordinator)->NotifyUnmuted());
}

template <typename D> Windows::ApplicationModel::Calls::VoipPhoneCall consume_Windows_ApplicationModel_Calls_IVoipCallCoordinator<D>::RequestOutgoingUpgradeToVideoCall(GUID const& callUpgradeGuid, param::hstring const& context, param::hstring const& contactName, param::hstring const& serviceName) const
{
    Windows::ApplicationModel::Calls::VoipPhoneCall call{ nullptr };
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipCallCoordinator)->RequestOutgoingUpgradeToVideoCall(get_abi(callUpgradeGuid), get_abi(context), get_abi(contactName), get_abi(serviceName), put_abi(call)));
    return call;
}

template <typename D> Windows::ApplicationModel::Calls::VoipPhoneCall consume_Windows_ApplicationModel_Calls_IVoipCallCoordinator<D>::RequestIncomingUpgradeToVideoCall(param::hstring const& context, param::hstring const& contactName, param::hstring const& contactNumber, Windows::Foundation::Uri const& contactImage, param::hstring const& serviceName, Windows::Foundation::Uri const& brandingImage, param::hstring const& callDetails, Windows::Foundation::Uri const& ringtone, Windows::Foundation::TimeSpan const& ringTimeout) const
{
    Windows::ApplicationModel::Calls::VoipPhoneCall call{ nullptr };
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipCallCoordinator)->RequestIncomingUpgradeToVideoCall(get_abi(context), get_abi(contactName), get_abi(contactNumber), get_abi(contactImage), get_abi(serviceName), get_abi(brandingImage), get_abi(callDetails), get_abi(ringtone), get_abi(ringTimeout), put_abi(call)));
    return call;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IVoipCallCoordinator<D>::TerminateCellularCall(GUID const& callUpgradeGuid) const
{
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipCallCoordinator)->TerminateCellularCall(get_abi(callUpgradeGuid)));
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IVoipCallCoordinator<D>::CancelUpgrade(GUID const& callUpgradeGuid) const
{
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipCallCoordinator)->CancelUpgrade(get_abi(callUpgradeGuid)));
}

template <typename D> Windows::ApplicationModel::Calls::VoipPhoneCall consume_Windows_ApplicationModel_Calls_IVoipCallCoordinator2<D>::SetupNewAcceptedCall(param::hstring const& context, param::hstring const& contactName, param::hstring const& contactNumber, param::hstring const& serviceName, Windows::ApplicationModel::Calls::VoipPhoneCallMedia const& media) const
{
    Windows::ApplicationModel::Calls::VoipPhoneCall call{ nullptr };
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipCallCoordinator2)->SetupNewAcceptedCall(get_abi(context), get_abi(contactName), get_abi(contactNumber), get_abi(serviceName), get_abi(media), put_abi(call)));
    return call;
}

template <typename D> Windows::ApplicationModel::Calls::VoipCallCoordinator consume_Windows_ApplicationModel_Calls_IVoipCallCoordinatorStatics<D>::GetDefault() const
{
    Windows::ApplicationModel::Calls::VoipCallCoordinator coordinator{ nullptr };
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipCallCoordinatorStatics)->GetDefault(put_abi(coordinator)));
    return coordinator;
}

template <typename D> event_token consume_Windows_ApplicationModel_Calls_IVoipPhoneCall<D>::EndRequested(Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipPhoneCall, Windows::ApplicationModel::Calls::CallStateChangeEventArgs> const& handler) const
{
    event_token token{};
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipPhoneCall)->add_EndRequested(get_abi(handler), put_abi(token)));
    return token;
}

template <typename D> event_revoker<Windows::ApplicationModel::Calls::IVoipPhoneCall> consume_Windows_ApplicationModel_Calls_IVoipPhoneCall<D>::EndRequested(auto_revoke_t, Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipPhoneCall, Windows::ApplicationModel::Calls::CallStateChangeEventArgs> const& handler) const
{
    return impl::make_event_revoker<D, Windows::ApplicationModel::Calls::IVoipPhoneCall>(this, &abi_t<Windows::ApplicationModel::Calls::IVoipPhoneCall>::remove_EndRequested, EndRequested(handler));
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IVoipPhoneCall<D>::EndRequested(event_token const& token) const
{
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipPhoneCall)->remove_EndRequested(get_abi(token)));
}

template <typename D> event_token consume_Windows_ApplicationModel_Calls_IVoipPhoneCall<D>::HoldRequested(Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipPhoneCall, Windows::ApplicationModel::Calls::CallStateChangeEventArgs> const& handler) const
{
    event_token token{};
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipPhoneCall)->add_HoldRequested(get_abi(handler), put_abi(token)));
    return token;
}

template <typename D> event_revoker<Windows::ApplicationModel::Calls::IVoipPhoneCall> consume_Windows_ApplicationModel_Calls_IVoipPhoneCall<D>::HoldRequested(auto_revoke_t, Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipPhoneCall, Windows::ApplicationModel::Calls::CallStateChangeEventArgs> const& handler) const
{
    return impl::make_event_revoker<D, Windows::ApplicationModel::Calls::IVoipPhoneCall>(this, &abi_t<Windows::ApplicationModel::Calls::IVoipPhoneCall>::remove_HoldRequested, HoldRequested(handler));
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IVoipPhoneCall<D>::HoldRequested(event_token const& token) const
{
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipPhoneCall)->remove_HoldRequested(get_abi(token)));
}

template <typename D> event_token consume_Windows_ApplicationModel_Calls_IVoipPhoneCall<D>::ResumeRequested(Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipPhoneCall, Windows::ApplicationModel::Calls::CallStateChangeEventArgs> const& handler) const
{
    event_token token{};
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipPhoneCall)->add_ResumeRequested(get_abi(handler), put_abi(token)));
    return token;
}

template <typename D> event_revoker<Windows::ApplicationModel::Calls::IVoipPhoneCall> consume_Windows_ApplicationModel_Calls_IVoipPhoneCall<D>::ResumeRequested(auto_revoke_t, Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipPhoneCall, Windows::ApplicationModel::Calls::CallStateChangeEventArgs> const& handler) const
{
    return impl::make_event_revoker<D, Windows::ApplicationModel::Calls::IVoipPhoneCall>(this, &abi_t<Windows::ApplicationModel::Calls::IVoipPhoneCall>::remove_ResumeRequested, ResumeRequested(handler));
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IVoipPhoneCall<D>::ResumeRequested(event_token const& token) const
{
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipPhoneCall)->remove_ResumeRequested(get_abi(token)));
}

template <typename D> event_token consume_Windows_ApplicationModel_Calls_IVoipPhoneCall<D>::AnswerRequested(Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipPhoneCall, Windows::ApplicationModel::Calls::CallAnswerEventArgs> const& acceptHandler) const
{
    event_token token{};
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipPhoneCall)->add_AnswerRequested(get_abi(acceptHandler), put_abi(token)));
    return token;
}

template <typename D> event_revoker<Windows::ApplicationModel::Calls::IVoipPhoneCall> consume_Windows_ApplicationModel_Calls_IVoipPhoneCall<D>::AnswerRequested(auto_revoke_t, Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipPhoneCall, Windows::ApplicationModel::Calls::CallAnswerEventArgs> const& acceptHandler) const
{
    return impl::make_event_revoker<D, Windows::ApplicationModel::Calls::IVoipPhoneCall>(this, &abi_t<Windows::ApplicationModel::Calls::IVoipPhoneCall>::remove_AnswerRequested, AnswerRequested(acceptHandler));
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IVoipPhoneCall<D>::AnswerRequested(event_token const& token) const
{
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipPhoneCall)->remove_AnswerRequested(get_abi(token)));
}

template <typename D> event_token consume_Windows_ApplicationModel_Calls_IVoipPhoneCall<D>::RejectRequested(Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipPhoneCall, Windows::ApplicationModel::Calls::CallRejectEventArgs> const& rejectHandler) const
{
    event_token token{};
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipPhoneCall)->add_RejectRequested(get_abi(rejectHandler), put_abi(token)));
    return token;
}

template <typename D> event_revoker<Windows::ApplicationModel::Calls::IVoipPhoneCall> consume_Windows_ApplicationModel_Calls_IVoipPhoneCall<D>::RejectRequested(auto_revoke_t, Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipPhoneCall, Windows::ApplicationModel::Calls::CallRejectEventArgs> const& rejectHandler) const
{
    return impl::make_event_revoker<D, Windows::ApplicationModel::Calls::IVoipPhoneCall>(this, &abi_t<Windows::ApplicationModel::Calls::IVoipPhoneCall>::remove_RejectRequested, RejectRequested(rejectHandler));
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IVoipPhoneCall<D>::RejectRequested(event_token const& token) const
{
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipPhoneCall)->remove_RejectRequested(get_abi(token)));
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IVoipPhoneCall<D>::NotifyCallHeld() const
{
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipPhoneCall)->NotifyCallHeld());
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IVoipPhoneCall<D>::NotifyCallActive() const
{
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipPhoneCall)->NotifyCallActive());
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IVoipPhoneCall<D>::NotifyCallEnded() const
{
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipPhoneCall)->NotifyCallEnded());
}

template <typename D> hstring consume_Windows_ApplicationModel_Calls_IVoipPhoneCall<D>::ContactName() const noexcept
{
    hstring value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipPhoneCall)->get_ContactName(put_abi(value)));
    return value;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IVoipPhoneCall<D>::ContactName(param::hstring const& value) const noexcept
{
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipPhoneCall)->put_ContactName(get_abi(value)));
}

template <typename D> Windows::Foundation::DateTime consume_Windows_ApplicationModel_Calls_IVoipPhoneCall<D>::StartTime() const noexcept
{
    Windows::Foundation::DateTime value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipPhoneCall)->get_StartTime(put_abi(value)));
    return value;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IVoipPhoneCall<D>::StartTime(Windows::Foundation::DateTime const& value) const noexcept
{
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipPhoneCall)->put_StartTime(get_abi(value)));
}

template <typename D> Windows::ApplicationModel::Calls::VoipPhoneCallMedia consume_Windows_ApplicationModel_Calls_IVoipPhoneCall<D>::CallMedia() const noexcept
{
    Windows::ApplicationModel::Calls::VoipPhoneCallMedia value{};
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipPhoneCall)->get_CallMedia(put_abi(value)));
    return value;
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IVoipPhoneCall<D>::CallMedia(Windows::ApplicationModel::Calls::VoipPhoneCallMedia const& value) const noexcept
{
    check_terminate(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipPhoneCall)->put_CallMedia(get_abi(value)));
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IVoipPhoneCall<D>::NotifyCallReady() const
{
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipPhoneCall)->NotifyCallReady());
}

template <typename D> void consume_Windows_ApplicationModel_Calls_IVoipPhoneCall2<D>::TryShowAppUI() const
{
    check_hresult(WINRT_SHIM(Windows::ApplicationModel::Calls::IVoipPhoneCall2)->TryShowAppUI());
}

template <typename D>
struct produce<D, Windows::ApplicationModel::Calls::ICallAnswerEventArgs> : produce_base<D, Windows::ApplicationModel::Calls::ICallAnswerEventArgs>
{
    HRESULT __stdcall get_AcceptedMedia(Windows::ApplicationModel::Calls::VoipPhoneCallMedia* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().AcceptedMedia());
        return S_OK;
    }
};

template <typename D>
struct produce<D, Windows::ApplicationModel::Calls::ICallRejectEventArgs> : produce_base<D, Windows::ApplicationModel::Calls::ICallRejectEventArgs>
{
    HRESULT __stdcall get_RejectReason(Windows::ApplicationModel::Calls::VoipPhoneCallRejectReason* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().RejectReason());
        return S_OK;
    }
};

template <typename D>
struct produce<D, Windows::ApplicationModel::Calls::ICallStateChangeEventArgs> : produce_base<D, Windows::ApplicationModel::Calls::ICallStateChangeEventArgs>
{
    HRESULT __stdcall get_State(Windows::ApplicationModel::Calls::VoipPhoneCallState* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().State());
        return S_OK;
    }
};

template <typename D>
struct produce<D, Windows::ApplicationModel::Calls::ILockScreenCallEndCallDeferral> : produce_base<D, Windows::ApplicationModel::Calls::ILockScreenCallEndCallDeferral>
{
    HRESULT __stdcall Complete() noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            this->shim().Complete();
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }
};

template <typename D>
struct produce<D, Windows::ApplicationModel::Calls::ILockScreenCallEndRequestedEventArgs> : produce_base<D, Windows::ApplicationModel::Calls::ILockScreenCallEndRequestedEventArgs>
{
    HRESULT __stdcall GetDeferral(::IUnknown** value) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *value = detach_abi(this->shim().GetDeferral());
            return S_OK;
        }
        catch (...)
        {
            *value = nullptr;
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall get_Deadline(Windows::Foundation::DateTime* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().Deadline());
        return S_OK;
    }
};

template <typename D>
struct produce<D, Windows::ApplicationModel::Calls::ILockScreenCallUI> : produce_base<D, Windows::ApplicationModel::Calls::ILockScreenCallUI>
{
    HRESULT __stdcall Dismiss() noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            this->shim().Dismiss();
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall add_EndRequested(::IUnknown* handler, event_token* token) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *token = detach_abi(this->shim().EndRequested(*reinterpret_cast<Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::LockScreenCallUI, Windows::ApplicationModel::Calls::LockScreenCallEndRequestedEventArgs> const*>(&handler)));
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall remove_EndRequested(event_token token) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            this->shim().EndRequested(*reinterpret_cast<event_token const*>(&token));
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall add_Closed(::IUnknown* handler, event_token* token) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *token = detach_abi(this->shim().Closed(*reinterpret_cast<Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::LockScreenCallUI, Windows::Foundation::IInspectable> const*>(&handler)));
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall remove_Closed(event_token token) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            this->shim().Closed(*reinterpret_cast<event_token const*>(&token));
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall get_CallTitle(HSTRING* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().CallTitle());
        return S_OK;
    }

    HRESULT __stdcall put_CallTitle(HSTRING value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        this->shim().CallTitle(*reinterpret_cast<hstring const*>(&value));
        return S_OK;
    }
};

template <typename D>
struct produce<D, Windows::ApplicationModel::Calls::IMuteChangeEventArgs> : produce_base<D, Windows::ApplicationModel::Calls::IMuteChangeEventArgs>
{
    HRESULT __stdcall get_Muted(bool* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().Muted());
        return S_OK;
    }
};

template <typename D>
struct produce<D, Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry> : produce_base<D, Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry>
{
    HRESULT __stdcall get_Id(HSTRING* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().Id());
        return S_OK;
    }

    HRESULT __stdcall get_Address(::IUnknown** value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().Address());
        return S_OK;
    }

    HRESULT __stdcall put_Address(::IUnknown* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        this->shim().Address(*reinterpret_cast<Windows::ApplicationModel::Calls::PhoneCallHistoryEntryAddress const*>(&value));
        return S_OK;
    }

    HRESULT __stdcall get_Duration(::IUnknown** value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().Duration());
        return S_OK;
    }

    HRESULT __stdcall put_Duration(::IUnknown* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        this->shim().Duration(*reinterpret_cast<Windows::Foundation::IReference<Windows::Foundation::TimeSpan> const*>(&value));
        return S_OK;
    }

    HRESULT __stdcall get_IsCallerIdBlocked(bool* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().IsCallerIdBlocked());
        return S_OK;
    }

    HRESULT __stdcall put_IsCallerIdBlocked(bool value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        this->shim().IsCallerIdBlocked(value);
        return S_OK;
    }

    HRESULT __stdcall get_IsEmergency(bool* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().IsEmergency());
        return S_OK;
    }

    HRESULT __stdcall put_IsEmergency(bool value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        this->shim().IsEmergency(value);
        return S_OK;
    }

    HRESULT __stdcall get_IsIncoming(bool* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().IsIncoming());
        return S_OK;
    }

    HRESULT __stdcall put_IsIncoming(bool value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        this->shim().IsIncoming(value);
        return S_OK;
    }

    HRESULT __stdcall get_IsMissed(bool* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().IsMissed());
        return S_OK;
    }

    HRESULT __stdcall put_IsMissed(bool value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        this->shim().IsMissed(value);
        return S_OK;
    }

    HRESULT __stdcall get_IsRinging(bool* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().IsRinging());
        return S_OK;
    }

    HRESULT __stdcall put_IsRinging(bool value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        this->shim().IsRinging(value);
        return S_OK;
    }

    HRESULT __stdcall get_IsSeen(bool* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().IsSeen());
        return S_OK;
    }

    HRESULT __stdcall put_IsSeen(bool value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        this->shim().IsSeen(value);
        return S_OK;
    }

    HRESULT __stdcall get_IsSuppressed(bool* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().IsSuppressed());
        return S_OK;
    }

    HRESULT __stdcall put_IsSuppressed(bool value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        this->shim().IsSuppressed(value);
        return S_OK;
    }

    HRESULT __stdcall get_IsVoicemail(bool* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().IsVoicemail());
        return S_OK;
    }

    HRESULT __stdcall put_IsVoicemail(bool value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        this->shim().IsVoicemail(value);
        return S_OK;
    }

    HRESULT __stdcall get_Media(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryMedia* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().Media());
        return S_OK;
    }

    HRESULT __stdcall put_Media(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryMedia value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        this->shim().Media(*reinterpret_cast<Windows::ApplicationModel::Calls::PhoneCallHistoryEntryMedia const*>(&value));
        return S_OK;
    }

    HRESULT __stdcall get_OtherAppReadAccess(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryOtherAppReadAccess* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().OtherAppReadAccess());
        return S_OK;
    }

    HRESULT __stdcall put_OtherAppReadAccess(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryOtherAppReadAccess value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        this->shim().OtherAppReadAccess(*reinterpret_cast<Windows::ApplicationModel::Calls::PhoneCallHistoryEntryOtherAppReadAccess const*>(&value));
        return S_OK;
    }

    HRESULT __stdcall get_RemoteId(HSTRING* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().RemoteId());
        return S_OK;
    }

    HRESULT __stdcall put_RemoteId(HSTRING value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        this->shim().RemoteId(*reinterpret_cast<hstring const*>(&value));
        return S_OK;
    }

    HRESULT __stdcall get_SourceDisplayName(HSTRING* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().SourceDisplayName());
        return S_OK;
    }

    HRESULT __stdcall get_SourceId(HSTRING* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().SourceId());
        return S_OK;
    }

    HRESULT __stdcall put_SourceId(HSTRING value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        this->shim().SourceId(*reinterpret_cast<hstring const*>(&value));
        return S_OK;
    }

    HRESULT __stdcall get_SourceIdKind(Windows::ApplicationModel::Calls::PhoneCallHistorySourceIdKind* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().SourceIdKind());
        return S_OK;
    }

    HRESULT __stdcall put_SourceIdKind(Windows::ApplicationModel::Calls::PhoneCallHistorySourceIdKind value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        this->shim().SourceIdKind(*reinterpret_cast<Windows::ApplicationModel::Calls::PhoneCallHistorySourceIdKind const*>(&value));
        return S_OK;
    }

    HRESULT __stdcall get_StartTime(Windows::Foundation::DateTime* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().StartTime());
        return S_OK;
    }

    HRESULT __stdcall put_StartTime(Windows::Foundation::DateTime value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        this->shim().StartTime(*reinterpret_cast<Windows::Foundation::DateTime const*>(&value));
        return S_OK;
    }
};

template <typename D>
struct produce<D, Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddress> : produce_base<D, Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddress>
{
    HRESULT __stdcall get_ContactId(HSTRING* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().ContactId());
        return S_OK;
    }

    HRESULT __stdcall put_ContactId(HSTRING value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        this->shim().ContactId(*reinterpret_cast<hstring const*>(&value));
        return S_OK;
    }

    HRESULT __stdcall get_DisplayName(HSTRING* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().DisplayName());
        return S_OK;
    }

    HRESULT __stdcall put_DisplayName(HSTRING value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        this->shim().DisplayName(*reinterpret_cast<hstring const*>(&value));
        return S_OK;
    }

    HRESULT __stdcall get_RawAddress(HSTRING* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().RawAddress());
        return S_OK;
    }

    HRESULT __stdcall put_RawAddress(HSTRING value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        this->shim().RawAddress(*reinterpret_cast<hstring const*>(&value));
        return S_OK;
    }

    HRESULT __stdcall get_RawAddressKind(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryRawAddressKind* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().RawAddressKind());
        return S_OK;
    }

    HRESULT __stdcall put_RawAddressKind(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryRawAddressKind value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        this->shim().RawAddressKind(*reinterpret_cast<Windows::ApplicationModel::Calls::PhoneCallHistoryEntryRawAddressKind const*>(&value));
        return S_OK;
    }
};

template <typename D>
struct produce<D, Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddressFactory> : produce_base<D, Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddressFactory>
{
    HRESULT __stdcall Create(HSTRING rawAddress, Windows::ApplicationModel::Calls::PhoneCallHistoryEntryRawAddressKind rawAddressKind, ::IUnknown** result) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *result = detach_abi(this->shim().Create(*reinterpret_cast<hstring const*>(&rawAddress), *reinterpret_cast<Windows::ApplicationModel::Calls::PhoneCallHistoryEntryRawAddressKind const*>(&rawAddressKind)));
            return S_OK;
        }
        catch (...)
        {
            *result = nullptr;
            return impl::to_hresult();
        }
    }
};

template <typename D>
struct produce<D, Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryQueryOptions> : produce_base<D, Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryQueryOptions>
{
    HRESULT __stdcall get_DesiredMedia(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryQueryDesiredMedia* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().DesiredMedia());
        return S_OK;
    }

    HRESULT __stdcall put_DesiredMedia(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryQueryDesiredMedia value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        this->shim().DesiredMedia(*reinterpret_cast<Windows::ApplicationModel::Calls::PhoneCallHistoryEntryQueryDesiredMedia const*>(&value));
        return S_OK;
    }

    HRESULT __stdcall get_SourceIds(::IUnknown** value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().SourceIds());
        return S_OK;
    }
};

template <typename D>
struct produce<D, Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryReader> : produce_base<D, Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryReader>
{
    HRESULT __stdcall ReadBatchAsync(::IUnknown** result) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *result = detach_abi(this->shim().ReadBatchAsync());
            return S_OK;
        }
        catch (...)
        {
            *result = nullptr;
            return impl::to_hresult();
        }
    }
};

template <typename D>
struct produce<D, Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerForUser> : produce_base<D, Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerForUser>
{
    HRESULT __stdcall RequestStoreAsync(Windows::ApplicationModel::Calls::PhoneCallHistoryStoreAccessType accessType, ::IUnknown** result) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *result = detach_abi(this->shim().RequestStoreAsync(*reinterpret_cast<Windows::ApplicationModel::Calls::PhoneCallHistoryStoreAccessType const*>(&accessType)));
            return S_OK;
        }
        catch (...)
        {
            *result = nullptr;
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall get_User(::IUnknown** value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().User());
        return S_OK;
    }
};

template <typename D>
struct produce<D, Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerStatics> : produce_base<D, Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerStatics>
{
    HRESULT __stdcall RequestStoreAsync(Windows::ApplicationModel::Calls::PhoneCallHistoryStoreAccessType accessType, ::IUnknown** result) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *result = detach_abi(this->shim().RequestStoreAsync(*reinterpret_cast<Windows::ApplicationModel::Calls::PhoneCallHistoryStoreAccessType const*>(&accessType)));
            return S_OK;
        }
        catch (...)
        {
            *result = nullptr;
            return impl::to_hresult();
        }
    }
};

template <typename D>
struct produce<D, Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerStatics2> : produce_base<D, Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerStatics2>
{
    HRESULT __stdcall GetForUser(::IUnknown* user, ::IUnknown** result) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *result = detach_abi(this->shim().GetForUser(*reinterpret_cast<Windows::System::User const*>(&user)));
            return S_OK;
        }
        catch (...)
        {
            *result = nullptr;
            return impl::to_hresult();
        }
    }
};

template <typename D>
struct produce<D, Windows::ApplicationModel::Calls::IPhoneCallHistoryStore> : produce_base<D, Windows::ApplicationModel::Calls::IPhoneCallHistoryStore>
{
    HRESULT __stdcall GetEntryAsync(HSTRING callHistoryEntryId, ::IUnknown** result) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *result = detach_abi(this->shim().GetEntryAsync(*reinterpret_cast<hstring const*>(&callHistoryEntryId)));
            return S_OK;
        }
        catch (...)
        {
            *result = nullptr;
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall GetEntryReader(::IUnknown** result) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *result = detach_abi(this->shim().GetEntryReader());
            return S_OK;
        }
        catch (...)
        {
            *result = nullptr;
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall GetEntryReaderWithOptions(::IUnknown* queryOptions, ::IUnknown** result) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *result = detach_abi(this->shim().GetEntryReader(*reinterpret_cast<Windows::ApplicationModel::Calls::PhoneCallHistoryEntryQueryOptions const*>(&queryOptions)));
            return S_OK;
        }
        catch (...)
        {
            *result = nullptr;
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall SaveEntryAsync(::IUnknown* callHistoryEntry, ::IUnknown** result) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *result = detach_abi(this->shim().SaveEntryAsync(*reinterpret_cast<Windows::ApplicationModel::Calls::PhoneCallHistoryEntry const*>(&callHistoryEntry)));
            return S_OK;
        }
        catch (...)
        {
            *result = nullptr;
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall DeleteEntryAsync(::IUnknown* callHistoryEntry, ::IUnknown** result) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *result = detach_abi(this->shim().DeleteEntryAsync(*reinterpret_cast<Windows::ApplicationModel::Calls::PhoneCallHistoryEntry const*>(&callHistoryEntry)));
            return S_OK;
        }
        catch (...)
        {
            *result = nullptr;
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall DeleteEntriesAsync(::IUnknown* callHistoryEntries, ::IUnknown** result) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *result = detach_abi(this->shim().DeleteEntriesAsync(*reinterpret_cast<Windows::Foundation::Collections::IIterable<Windows::ApplicationModel::Calls::PhoneCallHistoryEntry> const*>(&callHistoryEntries)));
            return S_OK;
        }
        catch (...)
        {
            *result = nullptr;
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall MarkEntryAsSeenAsync(::IUnknown* callHistoryEntry, ::IUnknown** result) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *result = detach_abi(this->shim().MarkEntryAsSeenAsync(*reinterpret_cast<Windows::ApplicationModel::Calls::PhoneCallHistoryEntry const*>(&callHistoryEntry)));
            return S_OK;
        }
        catch (...)
        {
            *result = nullptr;
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall MarkEntriesAsSeenAsync(::IUnknown* callHistoryEntries, ::IUnknown** result) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *result = detach_abi(this->shim().MarkEntriesAsSeenAsync(*reinterpret_cast<Windows::Foundation::Collections::IIterable<Windows::ApplicationModel::Calls::PhoneCallHistoryEntry> const*>(&callHistoryEntries)));
            return S_OK;
        }
        catch (...)
        {
            *result = nullptr;
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall GetUnseenCountAsync(::IUnknown** result) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *result = detach_abi(this->shim().GetUnseenCountAsync());
            return S_OK;
        }
        catch (...)
        {
            *result = nullptr;
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall MarkAllAsSeenAsync(::IUnknown** result) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *result = detach_abi(this->shim().MarkAllAsSeenAsync());
            return S_OK;
        }
        catch (...)
        {
            *result = nullptr;
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall GetSourcesUnseenCountAsync(::IUnknown* sourceIds, ::IUnknown** result) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *result = detach_abi(this->shim().GetSourcesUnseenCountAsync(*reinterpret_cast<Windows::Foundation::Collections::IIterable<hstring> const*>(&sourceIds)));
            return S_OK;
        }
        catch (...)
        {
            *result = nullptr;
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall MarkSourcesAsSeenAsync(::IUnknown* sourceIds, ::IUnknown** result) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *result = detach_abi(this->shim().MarkSourcesAsSeenAsync(*reinterpret_cast<Windows::Foundation::Collections::IIterable<hstring> const*>(&sourceIds)));
            return S_OK;
        }
        catch (...)
        {
            *result = nullptr;
            return impl::to_hresult();
        }
    }
};

template <typename D>
struct produce<D, Windows::ApplicationModel::Calls::IVoipCallCoordinator> : produce_base<D, Windows::ApplicationModel::Calls::IVoipCallCoordinator>
{
    HRESULT __stdcall ReserveCallResourcesAsync(HSTRING taskEntryPoint, ::IUnknown** operation) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *operation = detach_abi(this->shim().ReserveCallResourcesAsync(*reinterpret_cast<hstring const*>(&taskEntryPoint)));
            return S_OK;
        }
        catch (...)
        {
            *operation = nullptr;
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall add_MuteStateChanged(::IUnknown* muteChangeHandler, event_token* token) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *token = detach_abi(this->shim().MuteStateChanged(*reinterpret_cast<Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipCallCoordinator, Windows::ApplicationModel::Calls::MuteChangeEventArgs> const*>(&muteChangeHandler)));
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall remove_MuteStateChanged(event_token token) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            this->shim().MuteStateChanged(*reinterpret_cast<event_token const*>(&token));
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall RequestNewIncomingCall(HSTRING context, HSTRING contactName, HSTRING contactNumber, ::IUnknown* contactImage, HSTRING serviceName, ::IUnknown* brandingImage, HSTRING callDetails, ::IUnknown* ringtone, Windows::ApplicationModel::Calls::VoipPhoneCallMedia media, Windows::Foundation::TimeSpan ringTimeout, ::IUnknown** call) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *call = detach_abi(this->shim().RequestNewIncomingCall(*reinterpret_cast<hstring const*>(&context), *reinterpret_cast<hstring const*>(&contactName), *reinterpret_cast<hstring const*>(&contactNumber), *reinterpret_cast<Windows::Foundation::Uri const*>(&contactImage), *reinterpret_cast<hstring const*>(&serviceName), *reinterpret_cast<Windows::Foundation::Uri const*>(&brandingImage), *reinterpret_cast<hstring const*>(&callDetails), *reinterpret_cast<Windows::Foundation::Uri const*>(&ringtone), *reinterpret_cast<Windows::ApplicationModel::Calls::VoipPhoneCallMedia const*>(&media), *reinterpret_cast<Windows::Foundation::TimeSpan const*>(&ringTimeout)));
            return S_OK;
        }
        catch (...)
        {
            *call = nullptr;
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall RequestNewOutgoingCall(HSTRING context, HSTRING contactName, HSTRING serviceName, Windows::ApplicationModel::Calls::VoipPhoneCallMedia media, ::IUnknown** call) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *call = detach_abi(this->shim().RequestNewOutgoingCall(*reinterpret_cast<hstring const*>(&context), *reinterpret_cast<hstring const*>(&contactName), *reinterpret_cast<hstring const*>(&serviceName), *reinterpret_cast<Windows::ApplicationModel::Calls::VoipPhoneCallMedia const*>(&media)));
            return S_OK;
        }
        catch (...)
        {
            *call = nullptr;
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall NotifyMuted() noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            this->shim().NotifyMuted();
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall NotifyUnmuted() noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            this->shim().NotifyUnmuted();
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall RequestOutgoingUpgradeToVideoCall(GUID callUpgradeGuid, HSTRING context, HSTRING contactName, HSTRING serviceName, ::IUnknown** call) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *call = detach_abi(this->shim().RequestOutgoingUpgradeToVideoCall(*reinterpret_cast<GUID const*>(&callUpgradeGuid), *reinterpret_cast<hstring const*>(&context), *reinterpret_cast<hstring const*>(&contactName), *reinterpret_cast<hstring const*>(&serviceName)));
            return S_OK;
        }
        catch (...)
        {
            *call = nullptr;
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall RequestIncomingUpgradeToVideoCall(HSTRING context, HSTRING contactName, HSTRING contactNumber, ::IUnknown* contactImage, HSTRING serviceName, ::IUnknown* brandingImage, HSTRING callDetails, ::IUnknown* ringtone, Windows::Foundation::TimeSpan ringTimeout, ::IUnknown** call) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *call = detach_abi(this->shim().RequestIncomingUpgradeToVideoCall(*reinterpret_cast<hstring const*>(&context), *reinterpret_cast<hstring const*>(&contactName), *reinterpret_cast<hstring const*>(&contactNumber), *reinterpret_cast<Windows::Foundation::Uri const*>(&contactImage), *reinterpret_cast<hstring const*>(&serviceName), *reinterpret_cast<Windows::Foundation::Uri const*>(&brandingImage), *reinterpret_cast<hstring const*>(&callDetails), *reinterpret_cast<Windows::Foundation::Uri const*>(&ringtone), *reinterpret_cast<Windows::Foundation::TimeSpan const*>(&ringTimeout)));
            return S_OK;
        }
        catch (...)
        {
            *call = nullptr;
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall TerminateCellularCall(GUID callUpgradeGuid) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            this->shim().TerminateCellularCall(*reinterpret_cast<GUID const*>(&callUpgradeGuid));
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall CancelUpgrade(GUID callUpgradeGuid) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            this->shim().CancelUpgrade(*reinterpret_cast<GUID const*>(&callUpgradeGuid));
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }
};

template <typename D>
struct produce<D, Windows::ApplicationModel::Calls::IVoipCallCoordinator2> : produce_base<D, Windows::ApplicationModel::Calls::IVoipCallCoordinator2>
{
    HRESULT __stdcall SetupNewAcceptedCall(HSTRING context, HSTRING contactName, HSTRING contactNumber, HSTRING serviceName, Windows::ApplicationModel::Calls::VoipPhoneCallMedia media, ::IUnknown** call) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *call = detach_abi(this->shim().SetupNewAcceptedCall(*reinterpret_cast<hstring const*>(&context), *reinterpret_cast<hstring const*>(&contactName), *reinterpret_cast<hstring const*>(&contactNumber), *reinterpret_cast<hstring const*>(&serviceName), *reinterpret_cast<Windows::ApplicationModel::Calls::VoipPhoneCallMedia const*>(&media)));
            return S_OK;
        }
        catch (...)
        {
            *call = nullptr;
            return impl::to_hresult();
        }
    }
};

template <typename D>
struct produce<D, Windows::ApplicationModel::Calls::IVoipCallCoordinatorStatics> : produce_base<D, Windows::ApplicationModel::Calls::IVoipCallCoordinatorStatics>
{
    HRESULT __stdcall GetDefault(::IUnknown** coordinator) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *coordinator = detach_abi(this->shim().GetDefault());
            return S_OK;
        }
        catch (...)
        {
            *coordinator = nullptr;
            return impl::to_hresult();
        }
    }
};

template <typename D>
struct produce<D, Windows::ApplicationModel::Calls::IVoipPhoneCall> : produce_base<D, Windows::ApplicationModel::Calls::IVoipPhoneCall>
{
    HRESULT __stdcall add_EndRequested(::IUnknown* handler, event_token* token) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *token = detach_abi(this->shim().EndRequested(*reinterpret_cast<Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipPhoneCall, Windows::ApplicationModel::Calls::CallStateChangeEventArgs> const*>(&handler)));
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall remove_EndRequested(event_token token) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            this->shim().EndRequested(*reinterpret_cast<event_token const*>(&token));
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall add_HoldRequested(::IUnknown* handler, event_token* token) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *token = detach_abi(this->shim().HoldRequested(*reinterpret_cast<Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipPhoneCall, Windows::ApplicationModel::Calls::CallStateChangeEventArgs> const*>(&handler)));
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall remove_HoldRequested(event_token token) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            this->shim().HoldRequested(*reinterpret_cast<event_token const*>(&token));
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall add_ResumeRequested(::IUnknown* handler, event_token* token) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *token = detach_abi(this->shim().ResumeRequested(*reinterpret_cast<Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipPhoneCall, Windows::ApplicationModel::Calls::CallStateChangeEventArgs> const*>(&handler)));
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall remove_ResumeRequested(event_token token) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            this->shim().ResumeRequested(*reinterpret_cast<event_token const*>(&token));
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall add_AnswerRequested(::IUnknown* acceptHandler, event_token* token) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *token = detach_abi(this->shim().AnswerRequested(*reinterpret_cast<Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipPhoneCall, Windows::ApplicationModel::Calls::CallAnswerEventArgs> const*>(&acceptHandler)));
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall remove_AnswerRequested(event_token token) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            this->shim().AnswerRequested(*reinterpret_cast<event_token const*>(&token));
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall add_RejectRequested(::IUnknown* rejectHandler, event_token* token) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *token = detach_abi(this->shim().RejectRequested(*reinterpret_cast<Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipPhoneCall, Windows::ApplicationModel::Calls::CallRejectEventArgs> const*>(&rejectHandler)));
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall remove_RejectRequested(event_token token) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            this->shim().RejectRequested(*reinterpret_cast<event_token const*>(&token));
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall NotifyCallHeld() noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            this->shim().NotifyCallHeld();
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall NotifyCallActive() noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            this->shim().NotifyCallActive();
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall NotifyCallEnded() noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            this->shim().NotifyCallEnded();
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall get_ContactName(HSTRING* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().ContactName());
        return S_OK;
    }

    HRESULT __stdcall put_ContactName(HSTRING value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        this->shim().ContactName(*reinterpret_cast<hstring const*>(&value));
        return S_OK;
    }

    HRESULT __stdcall get_StartTime(Windows::Foundation::DateTime* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().StartTime());
        return S_OK;
    }

    HRESULT __stdcall put_StartTime(Windows::Foundation::DateTime value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        this->shim().StartTime(*reinterpret_cast<Windows::Foundation::DateTime const*>(&value));
        return S_OK;
    }

    HRESULT __stdcall get_CallMedia(Windows::ApplicationModel::Calls::VoipPhoneCallMedia* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().CallMedia());
        return S_OK;
    }

    HRESULT __stdcall put_CallMedia(Windows::ApplicationModel::Calls::VoipPhoneCallMedia value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        this->shim().CallMedia(*reinterpret_cast<Windows::ApplicationModel::Calls::VoipPhoneCallMedia const*>(&value));
        return S_OK;
    }

    HRESULT __stdcall NotifyCallReady() noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            this->shim().NotifyCallReady();
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }
};

template <typename D>
struct produce<D, Windows::ApplicationModel::Calls::IVoipPhoneCall2> : produce_base<D, Windows::ApplicationModel::Calls::IVoipPhoneCall2>
{
    HRESULT __stdcall TryShowAppUI() noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            this->shim().TryShowAppUI();
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }
};

}

WINRT_EXPORT namespace winrt::Windows::ApplicationModel::Calls {

inline PhoneCallHistoryEntry::PhoneCallHistoryEntry() :
    PhoneCallHistoryEntry(activate_instance<PhoneCallHistoryEntry>())
{}

inline PhoneCallHistoryEntryAddress::PhoneCallHistoryEntryAddress() :
    PhoneCallHistoryEntryAddress(activate_instance<PhoneCallHistoryEntryAddress>())
{}

inline PhoneCallHistoryEntryAddress::PhoneCallHistoryEntryAddress(param::hstring const& rawAddress, Windows::ApplicationModel::Calls::PhoneCallHistoryEntryRawAddressKind const& rawAddressKind) :
    PhoneCallHistoryEntryAddress(get_activation_factory<PhoneCallHistoryEntryAddress, Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddressFactory>().Create(rawAddress, rawAddressKind))
{}

inline PhoneCallHistoryEntryQueryOptions::PhoneCallHistoryEntryQueryOptions() :
    PhoneCallHistoryEntryQueryOptions(activate_instance<PhoneCallHistoryEntryQueryOptions>())
{}

inline Windows::Foundation::IAsyncOperation<Windows::ApplicationModel::Calls::PhoneCallHistoryStore> PhoneCallHistoryManager::RequestStoreAsync(Windows::ApplicationModel::Calls::PhoneCallHistoryStoreAccessType const& accessType)
{
    return get_activation_factory<PhoneCallHistoryManager, Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerStatics>().RequestStoreAsync(accessType);
}

inline Windows::ApplicationModel::Calls::PhoneCallHistoryManagerForUser PhoneCallHistoryManager::GetForUser(Windows::System::User const& user)
{
    return get_activation_factory<PhoneCallHistoryManager, Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerStatics2>().GetForUser(user);
}

inline Windows::ApplicationModel::Calls::VoipCallCoordinator VoipCallCoordinator::GetDefault()
{
    return get_activation_factory<VoipCallCoordinator, Windows::ApplicationModel::Calls::IVoipCallCoordinatorStatics>().GetDefault();
}

}

WINRT_EXPORT namespace std {

template<> struct hash<winrt::Windows::ApplicationModel::Calls::ICallAnswerEventArgs> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::ICallAnswerEventArgs> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::ICallRejectEventArgs> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::ICallRejectEventArgs> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::ICallStateChangeEventArgs> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::ICallStateChangeEventArgs> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::ILockScreenCallEndCallDeferral> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::ILockScreenCallEndCallDeferral> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::ILockScreenCallEndRequestedEventArgs> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::ILockScreenCallEndRequestedEventArgs> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::ILockScreenCallUI> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::ILockScreenCallUI> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::IMuteChangeEventArgs> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::IMuteChangeEventArgs> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddress> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddress> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddressFactory> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddressFactory> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryQueryOptions> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryQueryOptions> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryReader> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryReader> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerForUser> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerForUser> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerStatics> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerStatics> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerStatics2> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerStatics2> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::IPhoneCallHistoryStore> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::IPhoneCallHistoryStore> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::IVoipCallCoordinator> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::IVoipCallCoordinator> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::IVoipCallCoordinator2> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::IVoipCallCoordinator2> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::IVoipCallCoordinatorStatics> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::IVoipCallCoordinatorStatics> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::IVoipPhoneCall> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::IVoipPhoneCall> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::IVoipPhoneCall2> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::IVoipPhoneCall2> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::CallAnswerEventArgs> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::CallAnswerEventArgs> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::CallRejectEventArgs> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::CallRejectEventArgs> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::CallStateChangeEventArgs> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::CallStateChangeEventArgs> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::LockScreenCallEndCallDeferral> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::LockScreenCallEndCallDeferral> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::LockScreenCallEndRequestedEventArgs> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::LockScreenCallEndRequestedEventArgs> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::LockScreenCallUI> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::LockScreenCallUI> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::MuteChangeEventArgs> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::MuteChangeEventArgs> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::PhoneCallHistoryEntry> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::PhoneCallHistoryEntry> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::PhoneCallHistoryEntryAddress> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::PhoneCallHistoryEntryAddress> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::PhoneCallHistoryEntryQueryOptions> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::PhoneCallHistoryEntryQueryOptions> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::PhoneCallHistoryEntryReader> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::PhoneCallHistoryEntryReader> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::PhoneCallHistoryManager> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::PhoneCallHistoryManager> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::PhoneCallHistoryManagerForUser> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::PhoneCallHistoryManagerForUser> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::PhoneCallHistoryStore> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::PhoneCallHistoryStore> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::VoipCallCoordinator> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::VoipCallCoordinator> {};

template<> struct hash<winrt::Windows::ApplicationModel::Calls::VoipPhoneCall> : 
    winrt::impl::impl_hash_unknown<winrt::Windows::ApplicationModel::Calls::VoipPhoneCall> {};

}

WINRT_WARNING_POP
