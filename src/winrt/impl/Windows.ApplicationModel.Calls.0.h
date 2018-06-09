// C++/WinRT v1.0.170906.1
// Copyright (c) 2017 Microsoft Corporation. All rights reserved.

#pragma once

WINRT_EXPORT namespace winrt::Windows::Foundation {

struct Uri;

}

WINRT_EXPORT namespace winrt::Windows::System {

struct User;

}

WINRT_EXPORT namespace winrt::Windows::ApplicationModel::Calls {

enum class PhoneCallHistoryEntryMedia : int32_t
{
    Audio = 0,
    Video = 1,
};

enum class PhoneCallHistoryEntryOtherAppReadAccess : int32_t
{
    Full = 0,
    SystemOnly = 1,
};

enum class PhoneCallHistoryEntryQueryDesiredMedia : uint32_t
{
    None = 0x0,
    Audio = 0x1,
    Video = 0x2,
    All = 0xFFFFFFFF,
};

enum class PhoneCallHistoryEntryRawAddressKind : int32_t
{
    PhoneNumber = 0,
    Custom = 1,
};

enum class PhoneCallHistorySourceIdKind : int32_t
{
    CellularPhoneLineId = 0,
    PackageFamilyName = 1,
};

enum class PhoneCallHistoryStoreAccessType : int32_t
{
    AppEntriesReadWrite = 0,
    AllEntriesLimitedReadWrite = 1,
    AllEntriesReadWrite = 2,
};

enum class VoipPhoneCallMedia : uint32_t
{
    None = 0x0,
    Audio = 0x1,
    Video = 0x2,
};

enum class VoipPhoneCallRejectReason : int32_t
{
    UserIgnored = 0,
    TimedOut = 1,
    OtherIncomingCall = 2,
    EmergencyCallExists = 3,
    InvalidCallState = 4,
};

enum class VoipPhoneCallResourceReservationStatus : int32_t
{
    Success = 0,
    ResourcesNotAvailable = 1,
};

enum class VoipPhoneCallState : int32_t
{
    Ended = 0,
    Held = 1,
    Active = 2,
    Incoming = 3,
    Outgoing = 4,
};

struct ICallAnswerEventArgs;
struct ICallRejectEventArgs;
struct ICallStateChangeEventArgs;
struct ILockScreenCallEndCallDeferral;
struct ILockScreenCallEndRequestedEventArgs;
struct ILockScreenCallUI;
struct IMuteChangeEventArgs;
struct IPhoneCallHistoryEntry;
struct IPhoneCallHistoryEntryAddress;
struct IPhoneCallHistoryEntryAddressFactory;
struct IPhoneCallHistoryEntryQueryOptions;
struct IPhoneCallHistoryEntryReader;
struct IPhoneCallHistoryManagerForUser;
struct IPhoneCallHistoryManagerStatics;
struct IPhoneCallHistoryManagerStatics2;
struct IPhoneCallHistoryStore;
struct IVoipCallCoordinator;
struct IVoipCallCoordinator2;
struct IVoipCallCoordinatorStatics;
struct IVoipPhoneCall;
struct IVoipPhoneCall2;
struct CallAnswerEventArgs;
struct CallRejectEventArgs;
struct CallStateChangeEventArgs;
struct LockScreenCallEndCallDeferral;
struct LockScreenCallEndRequestedEventArgs;
struct LockScreenCallUI;
struct MuteChangeEventArgs;
struct PhoneCallHistoryEntry;
struct PhoneCallHistoryEntryAddress;
struct PhoneCallHistoryEntryQueryOptions;
struct PhoneCallHistoryEntryReader;
struct PhoneCallHistoryManager;
struct PhoneCallHistoryManagerForUser;
struct PhoneCallHistoryStore;
struct VoipCallCoordinator;
struct VoipPhoneCall;

}

namespace winrt::impl {

template<> struct is_enum_flag<Windows::ApplicationModel::Calls::PhoneCallHistoryEntryQueryDesiredMedia> : std::true_type {};
template<> struct is_enum_flag<Windows::ApplicationModel::Calls::VoipPhoneCallMedia> : std::true_type {};
template <> struct category<Windows::ApplicationModel::Calls::ICallAnswerEventArgs>{ using type = interface_category; };
template <> struct category<Windows::ApplicationModel::Calls::ICallRejectEventArgs>{ using type = interface_category; };
template <> struct category<Windows::ApplicationModel::Calls::ICallStateChangeEventArgs>{ using type = interface_category; };
template <> struct category<Windows::ApplicationModel::Calls::ILockScreenCallEndCallDeferral>{ using type = interface_category; };
template <> struct category<Windows::ApplicationModel::Calls::ILockScreenCallEndRequestedEventArgs>{ using type = interface_category; };
template <> struct category<Windows::ApplicationModel::Calls::ILockScreenCallUI>{ using type = interface_category; };
template <> struct category<Windows::ApplicationModel::Calls::IMuteChangeEventArgs>{ using type = interface_category; };
template <> struct category<Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry>{ using type = interface_category; };
template <> struct category<Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddress>{ using type = interface_category; };
template <> struct category<Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddressFactory>{ using type = interface_category; };
template <> struct category<Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryQueryOptions>{ using type = interface_category; };
template <> struct category<Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryReader>{ using type = interface_category; };
template <> struct category<Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerForUser>{ using type = interface_category; };
template <> struct category<Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerStatics>{ using type = interface_category; };
template <> struct category<Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerStatics2>{ using type = interface_category; };
template <> struct category<Windows::ApplicationModel::Calls::IPhoneCallHistoryStore>{ using type = interface_category; };
template <> struct category<Windows::ApplicationModel::Calls::IVoipCallCoordinator>{ using type = interface_category; };
template <> struct category<Windows::ApplicationModel::Calls::IVoipCallCoordinator2>{ using type = interface_category; };
template <> struct category<Windows::ApplicationModel::Calls::IVoipCallCoordinatorStatics>{ using type = interface_category; };
template <> struct category<Windows::ApplicationModel::Calls::IVoipPhoneCall>{ using type = interface_category; };
template <> struct category<Windows::ApplicationModel::Calls::IVoipPhoneCall2>{ using type = interface_category; };
template <> struct category<Windows::ApplicationModel::Calls::CallAnswerEventArgs>{ using type = class_category; };
template <> struct category<Windows::ApplicationModel::Calls::CallRejectEventArgs>{ using type = class_category; };
template <> struct category<Windows::ApplicationModel::Calls::CallStateChangeEventArgs>{ using type = class_category; };
template <> struct category<Windows::ApplicationModel::Calls::LockScreenCallEndCallDeferral>{ using type = class_category; };
template <> struct category<Windows::ApplicationModel::Calls::LockScreenCallEndRequestedEventArgs>{ using type = class_category; };
template <> struct category<Windows::ApplicationModel::Calls::LockScreenCallUI>{ using type = class_category; };
template <> struct category<Windows::ApplicationModel::Calls::MuteChangeEventArgs>{ using type = class_category; };
template <> struct category<Windows::ApplicationModel::Calls::PhoneCallHistoryEntry>{ using type = class_category; };
template <> struct category<Windows::ApplicationModel::Calls::PhoneCallHistoryEntryAddress>{ using type = class_category; };
template <> struct category<Windows::ApplicationModel::Calls::PhoneCallHistoryEntryQueryOptions>{ using type = class_category; };
template <> struct category<Windows::ApplicationModel::Calls::PhoneCallHistoryEntryReader>{ using type = class_category; };
template <> struct category<Windows::ApplicationModel::Calls::PhoneCallHistoryManager>{ using type = class_category; };
template <> struct category<Windows::ApplicationModel::Calls::PhoneCallHistoryManagerForUser>{ using type = class_category; };
template <> struct category<Windows::ApplicationModel::Calls::PhoneCallHistoryStore>{ using type = class_category; };
template <> struct category<Windows::ApplicationModel::Calls::VoipCallCoordinator>{ using type = class_category; };
template <> struct category<Windows::ApplicationModel::Calls::VoipPhoneCall>{ using type = class_category; };
template <> struct category<Windows::ApplicationModel::Calls::PhoneCallHistoryEntryMedia>{ using type = enum_category; };
template <> struct category<Windows::ApplicationModel::Calls::PhoneCallHistoryEntryOtherAppReadAccess>{ using type = enum_category; };
template <> struct category<Windows::ApplicationModel::Calls::PhoneCallHistoryEntryQueryDesiredMedia>{ using type = enum_category; };
template <> struct category<Windows::ApplicationModel::Calls::PhoneCallHistoryEntryRawAddressKind>{ using type = enum_category; };
template <> struct category<Windows::ApplicationModel::Calls::PhoneCallHistorySourceIdKind>{ using type = enum_category; };
template <> struct category<Windows::ApplicationModel::Calls::PhoneCallHistoryStoreAccessType>{ using type = enum_category; };
template <> struct category<Windows::ApplicationModel::Calls::VoipPhoneCallMedia>{ using type = enum_category; };
template <> struct category<Windows::ApplicationModel::Calls::VoipPhoneCallRejectReason>{ using type = enum_category; };
template <> struct category<Windows::ApplicationModel::Calls::VoipPhoneCallResourceReservationStatus>{ using type = enum_category; };
template <> struct category<Windows::ApplicationModel::Calls::VoipPhoneCallState>{ using type = enum_category; };
template <> struct name<Windows::ApplicationModel::Calls::ICallAnswerEventArgs>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.ICallAnswerEventArgs" }; };
template <> struct name<Windows::ApplicationModel::Calls::ICallRejectEventArgs>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.ICallRejectEventArgs" }; };
template <> struct name<Windows::ApplicationModel::Calls::ICallStateChangeEventArgs>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.ICallStateChangeEventArgs" }; };
template <> struct name<Windows::ApplicationModel::Calls::ILockScreenCallEndCallDeferral>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.ILockScreenCallEndCallDeferral" }; };
template <> struct name<Windows::ApplicationModel::Calls::ILockScreenCallEndRequestedEventArgs>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.ILockScreenCallEndRequestedEventArgs" }; };
template <> struct name<Windows::ApplicationModel::Calls::ILockScreenCallUI>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.ILockScreenCallUI" }; };
template <> struct name<Windows::ApplicationModel::Calls::IMuteChangeEventArgs>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.IMuteChangeEventArgs" }; };
template <> struct name<Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.IPhoneCallHistoryEntry" }; };
template <> struct name<Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddress>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.IPhoneCallHistoryEntryAddress" }; };
template <> struct name<Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddressFactory>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.IPhoneCallHistoryEntryAddressFactory" }; };
template <> struct name<Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryQueryOptions>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.IPhoneCallHistoryEntryQueryOptions" }; };
template <> struct name<Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryReader>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.IPhoneCallHistoryEntryReader" }; };
template <> struct name<Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerForUser>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.IPhoneCallHistoryManagerForUser" }; };
template <> struct name<Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerStatics>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.IPhoneCallHistoryManagerStatics" }; };
template <> struct name<Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerStatics2>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.IPhoneCallHistoryManagerStatics2" }; };
template <> struct name<Windows::ApplicationModel::Calls::IPhoneCallHistoryStore>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.IPhoneCallHistoryStore" }; };
template <> struct name<Windows::ApplicationModel::Calls::IVoipCallCoordinator>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.IVoipCallCoordinator" }; };
template <> struct name<Windows::ApplicationModel::Calls::IVoipCallCoordinator2>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.IVoipCallCoordinator2" }; };
template <> struct name<Windows::ApplicationModel::Calls::IVoipCallCoordinatorStatics>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.IVoipCallCoordinatorStatics" }; };
template <> struct name<Windows::ApplicationModel::Calls::IVoipPhoneCall>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.IVoipPhoneCall" }; };
template <> struct name<Windows::ApplicationModel::Calls::IVoipPhoneCall2>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.IVoipPhoneCall2" }; };
template <> struct name<Windows::ApplicationModel::Calls::CallAnswerEventArgs>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.CallAnswerEventArgs" }; };
template <> struct name<Windows::ApplicationModel::Calls::CallRejectEventArgs>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.CallRejectEventArgs" }; };
template <> struct name<Windows::ApplicationModel::Calls::CallStateChangeEventArgs>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.CallStateChangeEventArgs" }; };
template <> struct name<Windows::ApplicationModel::Calls::LockScreenCallEndCallDeferral>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.LockScreenCallEndCallDeferral" }; };
template <> struct name<Windows::ApplicationModel::Calls::LockScreenCallEndRequestedEventArgs>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.LockScreenCallEndRequestedEventArgs" }; };
template <> struct name<Windows::ApplicationModel::Calls::LockScreenCallUI>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.LockScreenCallUI" }; };
template <> struct name<Windows::ApplicationModel::Calls::MuteChangeEventArgs>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.MuteChangeEventArgs" }; };
template <> struct name<Windows::ApplicationModel::Calls::PhoneCallHistoryEntry>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.PhoneCallHistoryEntry" }; };
template <> struct name<Windows::ApplicationModel::Calls::PhoneCallHistoryEntryAddress>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.PhoneCallHistoryEntryAddress" }; };
template <> struct name<Windows::ApplicationModel::Calls::PhoneCallHistoryEntryQueryOptions>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.PhoneCallHistoryEntryQueryOptions" }; };
template <> struct name<Windows::ApplicationModel::Calls::PhoneCallHistoryEntryReader>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.PhoneCallHistoryEntryReader" }; };
template <> struct name<Windows::ApplicationModel::Calls::PhoneCallHistoryManager>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.PhoneCallHistoryManager" }; };
template <> struct name<Windows::ApplicationModel::Calls::PhoneCallHistoryManagerForUser>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.PhoneCallHistoryManagerForUser" }; };
template <> struct name<Windows::ApplicationModel::Calls::PhoneCallHistoryStore>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.PhoneCallHistoryStore" }; };
template <> struct name<Windows::ApplicationModel::Calls::VoipCallCoordinator>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.VoipCallCoordinator" }; };
template <> struct name<Windows::ApplicationModel::Calls::VoipPhoneCall>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.VoipPhoneCall" }; };
template <> struct name<Windows::ApplicationModel::Calls::PhoneCallHistoryEntryMedia>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.PhoneCallHistoryEntryMedia" }; };
template <> struct name<Windows::ApplicationModel::Calls::PhoneCallHistoryEntryOtherAppReadAccess>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.PhoneCallHistoryEntryOtherAppReadAccess" }; };
template <> struct name<Windows::ApplicationModel::Calls::PhoneCallHistoryEntryQueryDesiredMedia>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.PhoneCallHistoryEntryQueryDesiredMedia" }; };
template <> struct name<Windows::ApplicationModel::Calls::PhoneCallHistoryEntryRawAddressKind>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.PhoneCallHistoryEntryRawAddressKind" }; };
template <> struct name<Windows::ApplicationModel::Calls::PhoneCallHistorySourceIdKind>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.PhoneCallHistorySourceIdKind" }; };
template <> struct name<Windows::ApplicationModel::Calls::PhoneCallHistoryStoreAccessType>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.PhoneCallHistoryStoreAccessType" }; };
template <> struct name<Windows::ApplicationModel::Calls::VoipPhoneCallMedia>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.VoipPhoneCallMedia" }; };
template <> struct name<Windows::ApplicationModel::Calls::VoipPhoneCallRejectReason>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.VoipPhoneCallRejectReason" }; };
template <> struct name<Windows::ApplicationModel::Calls::VoipPhoneCallResourceReservationStatus>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.VoipPhoneCallResourceReservationStatus" }; };
template <> struct name<Windows::ApplicationModel::Calls::VoipPhoneCallState>{ static constexpr auto & value{ L"Windows.ApplicationModel.Calls.VoipPhoneCallState" }; };
template <> struct guid<Windows::ApplicationModel::Calls::ICallAnswerEventArgs>{ static constexpr GUID value{ 0xFD789617,0x2DD7,0x4C8C,{ 0xB2,0xBD,0x95,0xD1,0x7A,0x5B,0xB7,0x33 } }; };
template <> struct guid<Windows::ApplicationModel::Calls::ICallRejectEventArgs>{ static constexpr GUID value{ 0xDA47FAD7,0x13D4,0x4D92,{ 0xA1,0xC2,0xB7,0x78,0x11,0xEE,0x37,0xEC } }; };
template <> struct guid<Windows::ApplicationModel::Calls::ICallStateChangeEventArgs>{ static constexpr GUID value{ 0xEAB2349E,0x66F5,0x47F9,{ 0x9F,0xB5,0x45,0x9C,0x51,0x98,0xC7,0x20 } }; };
template <> struct guid<Windows::ApplicationModel::Calls::ILockScreenCallEndCallDeferral>{ static constexpr GUID value{ 0x2DD7ED0D,0x98ED,0x4041,{ 0x96,0x32,0x50,0xFF,0x81,0x2B,0x77,0x3F } }; };
template <> struct guid<Windows::ApplicationModel::Calls::ILockScreenCallEndRequestedEventArgs>{ static constexpr GUID value{ 0x8190A363,0x6F27,0x46E9,{ 0xAE,0xB6,0xC0,0xAE,0x83,0xE4,0x7D,0xC7 } }; };
template <> struct guid<Windows::ApplicationModel::Calls::ILockScreenCallUI>{ static constexpr GUID value{ 0xC596FD8D,0x73C9,0x4A14,{ 0xB0,0x21,0xEC,0x1C,0x50,0xA3,0xB7,0x27 } }; };
template <> struct guid<Windows::ApplicationModel::Calls::IMuteChangeEventArgs>{ static constexpr GUID value{ 0x8585E159,0x0C41,0x432C,{ 0x81,0x4D,0xC5,0xF1,0xFD,0xF5,0x30,0xBE } }; };
template <> struct guid<Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry>{ static constexpr GUID value{ 0xFAB0E129,0x32A4,0x4B85,{ 0x83,0xD1,0xF9,0x0D,0x8C,0x23,0xA8,0x57 } }; };
template <> struct guid<Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddress>{ static constexpr GUID value{ 0x30F159DA,0x3955,0x4042,{ 0x84,0xE6,0x66,0xEE,0xBF,0x82,0xE6,0x7F } }; };
template <> struct guid<Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddressFactory>{ static constexpr GUID value{ 0xFB0FADBA,0xC7F0,0x4BB6,{ 0x9F,0x6B,0xBA,0x5D,0x73,0x20,0x9A,0xCA } }; };
template <> struct guid<Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryQueryOptions>{ static constexpr GUID value{ 0x9C5FE15C,0x8BED,0x40CA,{ 0xB0,0x6E,0xC4,0xCA,0x8E,0xAE,0x5C,0x87 } }; };
template <> struct guid<Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryReader>{ static constexpr GUID value{ 0x61ECE4BE,0x8D86,0x479F,{ 0x84,0x04,0xA9,0x84,0x69,0x20,0xFE,0xE6 } }; };
template <> struct guid<Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerForUser>{ static constexpr GUID value{ 0xD925C523,0xF55F,0x4353,{ 0x9D,0xB4,0x02,0x05,0xA5,0x26,0x5A,0x55 } }; };
template <> struct guid<Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerStatics>{ static constexpr GUID value{ 0xF5A6DA39,0xB31F,0x4F45,{ 0xAC,0x8E,0x1B,0x08,0x89,0x3C,0x1B,0x50 } }; };
template <> struct guid<Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerStatics2>{ static constexpr GUID value{ 0xEFD474F0,0xA2DB,0x4188,{ 0x9E,0x92,0xBC,0x3C,0xFA,0x68,0x13,0xCF } }; };
template <> struct guid<Windows::ApplicationModel::Calls::IPhoneCallHistoryStore>{ static constexpr GUID value{ 0x2F907DB8,0xB40E,0x422B,{ 0x85,0x45,0xCB,0x19,0x10,0xA6,0x1C,0x52 } }; };
template <> struct guid<Windows::ApplicationModel::Calls::IVoipCallCoordinator>{ static constexpr GUID value{ 0x4F118BCF,0xE8EF,0x4434,{ 0x9C,0x5F,0xA8,0xD8,0x93,0xFA,0xFE,0x79 } }; };
template <> struct guid<Windows::ApplicationModel::Calls::IVoipCallCoordinator2>{ static constexpr GUID value{ 0xBEB4A9F3,0xC704,0x4234,{ 0x89,0xCE,0xE8,0x8C,0xC0,0xD2,0x8F,0xBE } }; };
template <> struct guid<Windows::ApplicationModel::Calls::IVoipCallCoordinatorStatics>{ static constexpr GUID value{ 0x7F5D1F2B,0xE04A,0x4D10,{ 0xB3,0x1A,0xA5,0x5C,0x92,0x2C,0xC2,0xFB } }; };
template <> struct guid<Windows::ApplicationModel::Calls::IVoipPhoneCall>{ static constexpr GUID value{ 0x6CF1F19A,0x7794,0x4A5A,{ 0x8C,0x68,0xAE,0x87,0x94,0x7A,0x69,0x90 } }; };
template <> struct guid<Windows::ApplicationModel::Calls::IVoipPhoneCall2>{ static constexpr GUID value{ 0x741B46E1,0x245F,0x41F3,{ 0x93,0x99,0x31,0x41,0xD2,0x5B,0x52,0xE3 } }; };
template <> struct default_interface<Windows::ApplicationModel::Calls::CallAnswerEventArgs>{ using type = Windows::ApplicationModel::Calls::ICallAnswerEventArgs; };
template <> struct default_interface<Windows::ApplicationModel::Calls::CallRejectEventArgs>{ using type = Windows::ApplicationModel::Calls::ICallRejectEventArgs; };
template <> struct default_interface<Windows::ApplicationModel::Calls::CallStateChangeEventArgs>{ using type = Windows::ApplicationModel::Calls::ICallStateChangeEventArgs; };
template <> struct default_interface<Windows::ApplicationModel::Calls::LockScreenCallEndCallDeferral>{ using type = Windows::ApplicationModel::Calls::ILockScreenCallEndCallDeferral; };
template <> struct default_interface<Windows::ApplicationModel::Calls::LockScreenCallEndRequestedEventArgs>{ using type = Windows::ApplicationModel::Calls::ILockScreenCallEndRequestedEventArgs; };
template <> struct default_interface<Windows::ApplicationModel::Calls::LockScreenCallUI>{ using type = Windows::ApplicationModel::Calls::ILockScreenCallUI; };
template <> struct default_interface<Windows::ApplicationModel::Calls::MuteChangeEventArgs>{ using type = Windows::ApplicationModel::Calls::IMuteChangeEventArgs; };
template <> struct default_interface<Windows::ApplicationModel::Calls::PhoneCallHistoryEntry>{ using type = Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry; };
template <> struct default_interface<Windows::ApplicationModel::Calls::PhoneCallHistoryEntryAddress>{ using type = Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddress; };
template <> struct default_interface<Windows::ApplicationModel::Calls::PhoneCallHistoryEntryQueryOptions>{ using type = Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryQueryOptions; };
template <> struct default_interface<Windows::ApplicationModel::Calls::PhoneCallHistoryEntryReader>{ using type = Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryReader; };
template <> struct default_interface<Windows::ApplicationModel::Calls::PhoneCallHistoryManagerForUser>{ using type = Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerForUser; };
template <> struct default_interface<Windows::ApplicationModel::Calls::PhoneCallHistoryStore>{ using type = Windows::ApplicationModel::Calls::IPhoneCallHistoryStore; };
template <> struct default_interface<Windows::ApplicationModel::Calls::VoipCallCoordinator>{ using type = Windows::ApplicationModel::Calls::IVoipCallCoordinator; };
template <> struct default_interface<Windows::ApplicationModel::Calls::VoipPhoneCall>{ using type = Windows::ApplicationModel::Calls::IVoipPhoneCall; };

template <typename D>
struct consume_Windows_ApplicationModel_Calls_ICallAnswerEventArgs
{
    Windows::ApplicationModel::Calls::VoipPhoneCallMedia AcceptedMedia() const noexcept;
};
template <> struct consume<Windows::ApplicationModel::Calls::ICallAnswerEventArgs> { template <typename D> using type = consume_Windows_ApplicationModel_Calls_ICallAnswerEventArgs<D>; };

template <typename D>
struct consume_Windows_ApplicationModel_Calls_ICallRejectEventArgs
{
    Windows::ApplicationModel::Calls::VoipPhoneCallRejectReason RejectReason() const noexcept;
};
template <> struct consume<Windows::ApplicationModel::Calls::ICallRejectEventArgs> { template <typename D> using type = consume_Windows_ApplicationModel_Calls_ICallRejectEventArgs<D>; };

template <typename D>
struct consume_Windows_ApplicationModel_Calls_ICallStateChangeEventArgs
{
    Windows::ApplicationModel::Calls::VoipPhoneCallState State() const noexcept;
};
template <> struct consume<Windows::ApplicationModel::Calls::ICallStateChangeEventArgs> { template <typename D> using type = consume_Windows_ApplicationModel_Calls_ICallStateChangeEventArgs<D>; };

template <typename D>
struct consume_Windows_ApplicationModel_Calls_ILockScreenCallEndCallDeferral
{
    void Complete() const;
};
template <> struct consume<Windows::ApplicationModel::Calls::ILockScreenCallEndCallDeferral> { template <typename D> using type = consume_Windows_ApplicationModel_Calls_ILockScreenCallEndCallDeferral<D>; };

template <typename D>
struct consume_Windows_ApplicationModel_Calls_ILockScreenCallEndRequestedEventArgs
{
    Windows::ApplicationModel::Calls::LockScreenCallEndCallDeferral GetDeferral() const;
    Windows::Foundation::DateTime Deadline() const noexcept;
};
template <> struct consume<Windows::ApplicationModel::Calls::ILockScreenCallEndRequestedEventArgs> { template <typename D> using type = consume_Windows_ApplicationModel_Calls_ILockScreenCallEndRequestedEventArgs<D>; };

template <typename D>
struct consume_Windows_ApplicationModel_Calls_ILockScreenCallUI
{
    void Dismiss() const;
    event_token EndRequested(Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::LockScreenCallUI, Windows::ApplicationModel::Calls::LockScreenCallEndRequestedEventArgs> const& handler) const;
    using EndRequested_revoker = event_revoker<Windows::ApplicationModel::Calls::ILockScreenCallUI>;
    EndRequested_revoker EndRequested(auto_revoke_t, Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::LockScreenCallUI, Windows::ApplicationModel::Calls::LockScreenCallEndRequestedEventArgs> const& handler) const;
    void EndRequested(event_token const& token) const;
    event_token Closed(Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::LockScreenCallUI, Windows::Foundation::IInspectable> const& handler) const;
    using Closed_revoker = event_revoker<Windows::ApplicationModel::Calls::ILockScreenCallUI>;
    Closed_revoker Closed(auto_revoke_t, Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::LockScreenCallUI, Windows::Foundation::IInspectable> const& handler) const;
    void Closed(event_token const& token) const;
    hstring CallTitle() const noexcept;
    void CallTitle(param::hstring const& value) const noexcept;
};
template <> struct consume<Windows::ApplicationModel::Calls::ILockScreenCallUI> { template <typename D> using type = consume_Windows_ApplicationModel_Calls_ILockScreenCallUI<D>; };

template <typename D>
struct consume_Windows_ApplicationModel_Calls_IMuteChangeEventArgs
{
    bool Muted() const noexcept;
};
template <> struct consume<Windows::ApplicationModel::Calls::IMuteChangeEventArgs> { template <typename D> using type = consume_Windows_ApplicationModel_Calls_IMuteChangeEventArgs<D>; };

template <typename D>
struct consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry
{
    hstring Id() const noexcept;
    Windows::ApplicationModel::Calls::PhoneCallHistoryEntryAddress Address() const noexcept;
    void Address(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryAddress const& value) const noexcept;
    Windows::Foundation::IReference<Windows::Foundation::TimeSpan> Duration() const noexcept;
    void Duration(optional<Windows::Foundation::TimeSpan> const& value) const noexcept;
    bool IsCallerIdBlocked() const noexcept;
    void IsCallerIdBlocked(bool value) const noexcept;
    bool IsEmergency() const noexcept;
    void IsEmergency(bool value) const noexcept;
    bool IsIncoming() const noexcept;
    void IsIncoming(bool value) const noexcept;
    bool IsMissed() const noexcept;
    void IsMissed(bool value) const noexcept;
    bool IsRinging() const noexcept;
    void IsRinging(bool value) const noexcept;
    bool IsSeen() const noexcept;
    void IsSeen(bool value) const noexcept;
    bool IsSuppressed() const noexcept;
    void IsSuppressed(bool value) const noexcept;
    bool IsVoicemail() const noexcept;
    void IsVoicemail(bool value) const noexcept;
    Windows::ApplicationModel::Calls::PhoneCallHistoryEntryMedia Media() const noexcept;
    void Media(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryMedia const& value) const noexcept;
    Windows::ApplicationModel::Calls::PhoneCallHistoryEntryOtherAppReadAccess OtherAppReadAccess() const noexcept;
    void OtherAppReadAccess(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryOtherAppReadAccess const& value) const noexcept;
    hstring RemoteId() const noexcept;
    void RemoteId(param::hstring const& value) const noexcept;
    hstring SourceDisplayName() const noexcept;
    hstring SourceId() const noexcept;
    void SourceId(param::hstring const& value) const noexcept;
    Windows::ApplicationModel::Calls::PhoneCallHistorySourceIdKind SourceIdKind() const noexcept;
    void SourceIdKind(Windows::ApplicationModel::Calls::PhoneCallHistorySourceIdKind const& value) const noexcept;
    Windows::Foundation::DateTime StartTime() const noexcept;
    void StartTime(Windows::Foundation::DateTime const& value) const noexcept;
};
template <> struct consume<Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry> { template <typename D> using type = consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntry<D>; };

template <typename D>
struct consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntryAddress
{
    hstring ContactId() const noexcept;
    void ContactId(param::hstring const& value) const noexcept;
    hstring DisplayName() const noexcept;
    void DisplayName(param::hstring const& value) const noexcept;
    hstring RawAddress() const noexcept;
    void RawAddress(param::hstring const& value) const noexcept;
    Windows::ApplicationModel::Calls::PhoneCallHistoryEntryRawAddressKind RawAddressKind() const noexcept;
    void RawAddressKind(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryRawAddressKind const& value) const noexcept;
};
template <> struct consume<Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddress> { template <typename D> using type = consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntryAddress<D>; };

template <typename D>
struct consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntryAddressFactory
{
    Windows::ApplicationModel::Calls::PhoneCallHistoryEntryAddress Create(param::hstring const& rawAddress, Windows::ApplicationModel::Calls::PhoneCallHistoryEntryRawAddressKind const& rawAddressKind) const;
};
template <> struct consume<Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddressFactory> { template <typename D> using type = consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntryAddressFactory<D>; };

template <typename D>
struct consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntryQueryOptions
{
    Windows::ApplicationModel::Calls::PhoneCallHistoryEntryQueryDesiredMedia DesiredMedia() const noexcept;
    void DesiredMedia(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryQueryDesiredMedia const& value) const noexcept;
    Windows::Foundation::Collections::IVector<hstring> SourceIds() const noexcept;
};
template <> struct consume<Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryQueryOptions> { template <typename D> using type = consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntryQueryOptions<D>; };

template <typename D>
struct consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntryReader
{
    Windows::Foundation::IAsyncOperation<Windows::Foundation::Collections::IVectorView<Windows::ApplicationModel::Calls::PhoneCallHistoryEntry>> ReadBatchAsync() const;
};
template <> struct consume<Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryReader> { template <typename D> using type = consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryEntryReader<D>; };

template <typename D>
struct consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryManagerForUser
{
    Windows::Foundation::IAsyncOperation<Windows::ApplicationModel::Calls::PhoneCallHistoryStore> RequestStoreAsync(Windows::ApplicationModel::Calls::PhoneCallHistoryStoreAccessType const& accessType) const;
    Windows::System::User User() const noexcept;
};
template <> struct consume<Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerForUser> { template <typename D> using type = consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryManagerForUser<D>; };

template <typename D>
struct consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryManagerStatics
{
    Windows::Foundation::IAsyncOperation<Windows::ApplicationModel::Calls::PhoneCallHistoryStore> RequestStoreAsync(Windows::ApplicationModel::Calls::PhoneCallHistoryStoreAccessType const& accessType) const;
};
template <> struct consume<Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerStatics> { template <typename D> using type = consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryManagerStatics<D>; };

template <typename D>
struct consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryManagerStatics2
{
    Windows::ApplicationModel::Calls::PhoneCallHistoryManagerForUser GetForUser(Windows::System::User const& user) const;
};
template <> struct consume<Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerStatics2> { template <typename D> using type = consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryManagerStatics2<D>; };

template <typename D>
struct consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryStore
{
    Windows::Foundation::IAsyncOperation<Windows::ApplicationModel::Calls::PhoneCallHistoryEntry> GetEntryAsync(param::hstring const& callHistoryEntryId) const;
    Windows::ApplicationModel::Calls::PhoneCallHistoryEntryReader GetEntryReader() const;
    Windows::ApplicationModel::Calls::PhoneCallHistoryEntryReader GetEntryReader(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryQueryOptions const& queryOptions) const;
    Windows::Foundation::IAsyncAction SaveEntryAsync(Windows::ApplicationModel::Calls::PhoneCallHistoryEntry const& callHistoryEntry) const;
    Windows::Foundation::IAsyncAction DeleteEntryAsync(Windows::ApplicationModel::Calls::PhoneCallHistoryEntry const& callHistoryEntry) const;
    Windows::Foundation::IAsyncAction DeleteEntriesAsync(param::async_iterable<Windows::ApplicationModel::Calls::PhoneCallHistoryEntry> const& callHistoryEntries) const;
    Windows::Foundation::IAsyncAction MarkEntryAsSeenAsync(Windows::ApplicationModel::Calls::PhoneCallHistoryEntry const& callHistoryEntry) const;
    Windows::Foundation::IAsyncAction MarkEntriesAsSeenAsync(param::async_iterable<Windows::ApplicationModel::Calls::PhoneCallHistoryEntry> const& callHistoryEntries) const;
    Windows::Foundation::IAsyncOperation<uint32_t> GetUnseenCountAsync() const;
    Windows::Foundation::IAsyncAction MarkAllAsSeenAsync() const;
    Windows::Foundation::IAsyncOperation<uint32_t> GetSourcesUnseenCountAsync(param::async_iterable<hstring> const& sourceIds) const;
    Windows::Foundation::IAsyncAction MarkSourcesAsSeenAsync(param::async_iterable<hstring> const& sourceIds) const;
};
template <> struct consume<Windows::ApplicationModel::Calls::IPhoneCallHistoryStore> { template <typename D> using type = consume_Windows_ApplicationModel_Calls_IPhoneCallHistoryStore<D>; };

template <typename D>
struct consume_Windows_ApplicationModel_Calls_IVoipCallCoordinator
{
    Windows::Foundation::IAsyncOperation<Windows::ApplicationModel::Calls::VoipPhoneCallResourceReservationStatus> ReserveCallResourcesAsync(param::hstring const& taskEntryPoint) const;
    event_token MuteStateChanged(Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipCallCoordinator, Windows::ApplicationModel::Calls::MuteChangeEventArgs> const& muteChangeHandler) const;
    using MuteStateChanged_revoker = event_revoker<Windows::ApplicationModel::Calls::IVoipCallCoordinator>;
    MuteStateChanged_revoker MuteStateChanged(auto_revoke_t, Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipCallCoordinator, Windows::ApplicationModel::Calls::MuteChangeEventArgs> const& muteChangeHandler) const;
    void MuteStateChanged(event_token const& token) const;
    Windows::ApplicationModel::Calls::VoipPhoneCall RequestNewIncomingCall(param::hstring const& context, param::hstring const& contactName, param::hstring const& contactNumber, Windows::Foundation::Uri const& contactImage, param::hstring const& serviceName, Windows::Foundation::Uri const& brandingImage, param::hstring const& callDetails, Windows::Foundation::Uri const& ringtone, Windows::ApplicationModel::Calls::VoipPhoneCallMedia const& media, Windows::Foundation::TimeSpan const& ringTimeout) const;
    Windows::ApplicationModel::Calls::VoipPhoneCall RequestNewOutgoingCall(param::hstring const& context, param::hstring const& contactName, param::hstring const& serviceName, Windows::ApplicationModel::Calls::VoipPhoneCallMedia const& media) const;
    void NotifyMuted() const;
    void NotifyUnmuted() const;
    Windows::ApplicationModel::Calls::VoipPhoneCall RequestOutgoingUpgradeToVideoCall(GUID const& callUpgradeGuid, param::hstring const& context, param::hstring const& contactName, param::hstring const& serviceName) const;
    Windows::ApplicationModel::Calls::VoipPhoneCall RequestIncomingUpgradeToVideoCall(param::hstring const& context, param::hstring const& contactName, param::hstring const& contactNumber, Windows::Foundation::Uri const& contactImage, param::hstring const& serviceName, Windows::Foundation::Uri const& brandingImage, param::hstring const& callDetails, Windows::Foundation::Uri const& ringtone, Windows::Foundation::TimeSpan const& ringTimeout) const;
    void TerminateCellularCall(GUID const& callUpgradeGuid) const;
    void CancelUpgrade(GUID const& callUpgradeGuid) const;
};
template <> struct consume<Windows::ApplicationModel::Calls::IVoipCallCoordinator> { template <typename D> using type = consume_Windows_ApplicationModel_Calls_IVoipCallCoordinator<D>; };

template <typename D>
struct consume_Windows_ApplicationModel_Calls_IVoipCallCoordinator2
{
    Windows::ApplicationModel::Calls::VoipPhoneCall SetupNewAcceptedCall(param::hstring const& context, param::hstring const& contactName, param::hstring const& contactNumber, param::hstring const& serviceName, Windows::ApplicationModel::Calls::VoipPhoneCallMedia const& media) const;
};
template <> struct consume<Windows::ApplicationModel::Calls::IVoipCallCoordinator2> { template <typename D> using type = consume_Windows_ApplicationModel_Calls_IVoipCallCoordinator2<D>; };

template <typename D>
struct consume_Windows_ApplicationModel_Calls_IVoipCallCoordinatorStatics
{
    Windows::ApplicationModel::Calls::VoipCallCoordinator GetDefault() const;
};
template <> struct consume<Windows::ApplicationModel::Calls::IVoipCallCoordinatorStatics> { template <typename D> using type = consume_Windows_ApplicationModel_Calls_IVoipCallCoordinatorStatics<D>; };

template <typename D>
struct consume_Windows_ApplicationModel_Calls_IVoipPhoneCall
{
    event_token EndRequested(Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipPhoneCall, Windows::ApplicationModel::Calls::CallStateChangeEventArgs> const& handler) const;
    using EndRequested_revoker = event_revoker<Windows::ApplicationModel::Calls::IVoipPhoneCall>;
    EndRequested_revoker EndRequested(auto_revoke_t, Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipPhoneCall, Windows::ApplicationModel::Calls::CallStateChangeEventArgs> const& handler) const;
    void EndRequested(event_token const& token) const;
    event_token HoldRequested(Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipPhoneCall, Windows::ApplicationModel::Calls::CallStateChangeEventArgs> const& handler) const;
    using HoldRequested_revoker = event_revoker<Windows::ApplicationModel::Calls::IVoipPhoneCall>;
    HoldRequested_revoker HoldRequested(auto_revoke_t, Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipPhoneCall, Windows::ApplicationModel::Calls::CallStateChangeEventArgs> const& handler) const;
    void HoldRequested(event_token const& token) const;
    event_token ResumeRequested(Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipPhoneCall, Windows::ApplicationModel::Calls::CallStateChangeEventArgs> const& handler) const;
    using ResumeRequested_revoker = event_revoker<Windows::ApplicationModel::Calls::IVoipPhoneCall>;
    ResumeRequested_revoker ResumeRequested(auto_revoke_t, Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipPhoneCall, Windows::ApplicationModel::Calls::CallStateChangeEventArgs> const& handler) const;
    void ResumeRequested(event_token const& token) const;
    event_token AnswerRequested(Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipPhoneCall, Windows::ApplicationModel::Calls::CallAnswerEventArgs> const& acceptHandler) const;
    using AnswerRequested_revoker = event_revoker<Windows::ApplicationModel::Calls::IVoipPhoneCall>;
    AnswerRequested_revoker AnswerRequested(auto_revoke_t, Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipPhoneCall, Windows::ApplicationModel::Calls::CallAnswerEventArgs> const& acceptHandler) const;
    void AnswerRequested(event_token const& token) const;
    event_token RejectRequested(Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipPhoneCall, Windows::ApplicationModel::Calls::CallRejectEventArgs> const& rejectHandler) const;
    using RejectRequested_revoker = event_revoker<Windows::ApplicationModel::Calls::IVoipPhoneCall>;
    RejectRequested_revoker RejectRequested(auto_revoke_t, Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Calls::VoipPhoneCall, Windows::ApplicationModel::Calls::CallRejectEventArgs> const& rejectHandler) const;
    void RejectRequested(event_token const& token) const;
    void NotifyCallHeld() const;
    void NotifyCallActive() const;
    void NotifyCallEnded() const;
    hstring ContactName() const noexcept;
    void ContactName(param::hstring const& value) const noexcept;
    Windows::Foundation::DateTime StartTime() const noexcept;
    void StartTime(Windows::Foundation::DateTime const& value) const noexcept;
    Windows::ApplicationModel::Calls::VoipPhoneCallMedia CallMedia() const noexcept;
    void CallMedia(Windows::ApplicationModel::Calls::VoipPhoneCallMedia const& value) const noexcept;
    void NotifyCallReady() const;
};
template <> struct consume<Windows::ApplicationModel::Calls::IVoipPhoneCall> { template <typename D> using type = consume_Windows_ApplicationModel_Calls_IVoipPhoneCall<D>; };

template <typename D>
struct consume_Windows_ApplicationModel_Calls_IVoipPhoneCall2
{
    void TryShowAppUI() const;
};
template <> struct consume<Windows::ApplicationModel::Calls::IVoipPhoneCall2> { template <typename D> using type = consume_Windows_ApplicationModel_Calls_IVoipPhoneCall2<D>; };

template <> struct abi<Windows::ApplicationModel::Calls::ICallAnswerEventArgs>{ struct type : ::IInspectable
{
    virtual HRESULT __stdcall get_AcceptedMedia(Windows::ApplicationModel::Calls::VoipPhoneCallMedia* value) = 0;
};};

template <> struct abi<Windows::ApplicationModel::Calls::ICallRejectEventArgs>{ struct type : ::IInspectable
{
    virtual HRESULT __stdcall get_RejectReason(Windows::ApplicationModel::Calls::VoipPhoneCallRejectReason* value) = 0;
};};

template <> struct abi<Windows::ApplicationModel::Calls::ICallStateChangeEventArgs>{ struct type : ::IInspectable
{
    virtual HRESULT __stdcall get_State(Windows::ApplicationModel::Calls::VoipPhoneCallState* value) = 0;
};};

template <> struct abi<Windows::ApplicationModel::Calls::ILockScreenCallEndCallDeferral>{ struct type : ::IInspectable
{
    virtual HRESULT __stdcall Complete() = 0;
};};

template <> struct abi<Windows::ApplicationModel::Calls::ILockScreenCallEndRequestedEventArgs>{ struct type : ::IInspectable
{
    virtual HRESULT __stdcall GetDeferral(::IUnknown** value) = 0;
    virtual HRESULT __stdcall get_Deadline(Windows::Foundation::DateTime* value) = 0;
};};

template <> struct abi<Windows::ApplicationModel::Calls::ILockScreenCallUI>{ struct type : ::IInspectable
{
    virtual HRESULT __stdcall Dismiss() = 0;
    virtual HRESULT __stdcall add_EndRequested(::IUnknown* handler, event_token* token) = 0;
    virtual HRESULT __stdcall remove_EndRequested(event_token token) = 0;
    virtual HRESULT __stdcall add_Closed(::IUnknown* handler, event_token* token) = 0;
    virtual HRESULT __stdcall remove_Closed(event_token token) = 0;
    virtual HRESULT __stdcall get_CallTitle(HSTRING* value) = 0;
    virtual HRESULT __stdcall put_CallTitle(HSTRING value) = 0;
};};

template <> struct abi<Windows::ApplicationModel::Calls::IMuteChangeEventArgs>{ struct type : ::IInspectable
{
    virtual HRESULT __stdcall get_Muted(bool* value) = 0;
};};

template <> struct abi<Windows::ApplicationModel::Calls::IPhoneCallHistoryEntry>{ struct type : ::IInspectable
{
    virtual HRESULT __stdcall get_Id(HSTRING* value) = 0;
    virtual HRESULT __stdcall get_Address(::IUnknown** value) = 0;
    virtual HRESULT __stdcall put_Address(::IUnknown* value) = 0;
    virtual HRESULT __stdcall get_Duration(::IUnknown** value) = 0;
    virtual HRESULT __stdcall put_Duration(::IUnknown* value) = 0;
    virtual HRESULT __stdcall get_IsCallerIdBlocked(bool* value) = 0;
    virtual HRESULT __stdcall put_IsCallerIdBlocked(bool value) = 0;
    virtual HRESULT __stdcall get_IsEmergency(bool* value) = 0;
    virtual HRESULT __stdcall put_IsEmergency(bool value) = 0;
    virtual HRESULT __stdcall get_IsIncoming(bool* value) = 0;
    virtual HRESULT __stdcall put_IsIncoming(bool value) = 0;
    virtual HRESULT __stdcall get_IsMissed(bool* value) = 0;
    virtual HRESULT __stdcall put_IsMissed(bool value) = 0;
    virtual HRESULT __stdcall get_IsRinging(bool* value) = 0;
    virtual HRESULT __stdcall put_IsRinging(bool value) = 0;
    virtual HRESULT __stdcall get_IsSeen(bool* value) = 0;
    virtual HRESULT __stdcall put_IsSeen(bool value) = 0;
    virtual HRESULT __stdcall get_IsSuppressed(bool* value) = 0;
    virtual HRESULT __stdcall put_IsSuppressed(bool value) = 0;
    virtual HRESULT __stdcall get_IsVoicemail(bool* value) = 0;
    virtual HRESULT __stdcall put_IsVoicemail(bool value) = 0;
    virtual HRESULT __stdcall get_Media(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryMedia* value) = 0;
    virtual HRESULT __stdcall put_Media(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryMedia value) = 0;
    virtual HRESULT __stdcall get_OtherAppReadAccess(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryOtherAppReadAccess* value) = 0;
    virtual HRESULT __stdcall put_OtherAppReadAccess(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryOtherAppReadAccess value) = 0;
    virtual HRESULT __stdcall get_RemoteId(HSTRING* value) = 0;
    virtual HRESULT __stdcall put_RemoteId(HSTRING value) = 0;
    virtual HRESULT __stdcall get_SourceDisplayName(HSTRING* value) = 0;
    virtual HRESULT __stdcall get_SourceId(HSTRING* value) = 0;
    virtual HRESULT __stdcall put_SourceId(HSTRING value) = 0;
    virtual HRESULT __stdcall get_SourceIdKind(Windows::ApplicationModel::Calls::PhoneCallHistorySourceIdKind* value) = 0;
    virtual HRESULT __stdcall put_SourceIdKind(Windows::ApplicationModel::Calls::PhoneCallHistorySourceIdKind value) = 0;
    virtual HRESULT __stdcall get_StartTime(Windows::Foundation::DateTime* value) = 0;
    virtual HRESULT __stdcall put_StartTime(Windows::Foundation::DateTime value) = 0;
};};

template <> struct abi<Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddress>{ struct type : ::IInspectable
{
    virtual HRESULT __stdcall get_ContactId(HSTRING* value) = 0;
    virtual HRESULT __stdcall put_ContactId(HSTRING value) = 0;
    virtual HRESULT __stdcall get_DisplayName(HSTRING* value) = 0;
    virtual HRESULT __stdcall put_DisplayName(HSTRING value) = 0;
    virtual HRESULT __stdcall get_RawAddress(HSTRING* value) = 0;
    virtual HRESULT __stdcall put_RawAddress(HSTRING value) = 0;
    virtual HRESULT __stdcall get_RawAddressKind(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryRawAddressKind* value) = 0;
    virtual HRESULT __stdcall put_RawAddressKind(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryRawAddressKind value) = 0;
};};

template <> struct abi<Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryAddressFactory>{ struct type : ::IInspectable
{
    virtual HRESULT __stdcall Create(HSTRING rawAddress, Windows::ApplicationModel::Calls::PhoneCallHistoryEntryRawAddressKind rawAddressKind, ::IUnknown** result) = 0;
};};

template <> struct abi<Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryQueryOptions>{ struct type : ::IInspectable
{
    virtual HRESULT __stdcall get_DesiredMedia(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryQueryDesiredMedia* value) = 0;
    virtual HRESULT __stdcall put_DesiredMedia(Windows::ApplicationModel::Calls::PhoneCallHistoryEntryQueryDesiredMedia value) = 0;
    virtual HRESULT __stdcall get_SourceIds(::IUnknown** value) = 0;
};};

template <> struct abi<Windows::ApplicationModel::Calls::IPhoneCallHistoryEntryReader>{ struct type : ::IInspectable
{
    virtual HRESULT __stdcall ReadBatchAsync(::IUnknown** result) = 0;
};};

template <> struct abi<Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerForUser>{ struct type : ::IInspectable
{
    virtual HRESULT __stdcall RequestStoreAsync(Windows::ApplicationModel::Calls::PhoneCallHistoryStoreAccessType accessType, ::IUnknown** result) = 0;
    virtual HRESULT __stdcall get_User(::IUnknown** value) = 0;
};};

template <> struct abi<Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerStatics>{ struct type : ::IInspectable
{
    virtual HRESULT __stdcall RequestStoreAsync(Windows::ApplicationModel::Calls::PhoneCallHistoryStoreAccessType accessType, ::IUnknown** result) = 0;
};};

template <> struct abi<Windows::ApplicationModel::Calls::IPhoneCallHistoryManagerStatics2>{ struct type : ::IInspectable
{
    virtual HRESULT __stdcall GetForUser(::IUnknown* user, ::IUnknown** result) = 0;
};};

template <> struct abi<Windows::ApplicationModel::Calls::IPhoneCallHistoryStore>{ struct type : ::IInspectable
{
    virtual HRESULT __stdcall GetEntryAsync(HSTRING callHistoryEntryId, ::IUnknown** result) = 0;
    virtual HRESULT __stdcall GetEntryReader(::IUnknown** result) = 0;
    virtual HRESULT __stdcall GetEntryReaderWithOptions(::IUnknown* queryOptions, ::IUnknown** result) = 0;
    virtual HRESULT __stdcall SaveEntryAsync(::IUnknown* callHistoryEntry, ::IUnknown** result) = 0;
    virtual HRESULT __stdcall DeleteEntryAsync(::IUnknown* callHistoryEntry, ::IUnknown** result) = 0;
    virtual HRESULT __stdcall DeleteEntriesAsync(::IUnknown* callHistoryEntries, ::IUnknown** result) = 0;
    virtual HRESULT __stdcall MarkEntryAsSeenAsync(::IUnknown* callHistoryEntry, ::IUnknown** result) = 0;
    virtual HRESULT __stdcall MarkEntriesAsSeenAsync(::IUnknown* callHistoryEntries, ::IUnknown** result) = 0;
    virtual HRESULT __stdcall GetUnseenCountAsync(::IUnknown** result) = 0;
    virtual HRESULT __stdcall MarkAllAsSeenAsync(::IUnknown** result) = 0;
    virtual HRESULT __stdcall GetSourcesUnseenCountAsync(::IUnknown* sourceIds, ::IUnknown** result) = 0;
    virtual HRESULT __stdcall MarkSourcesAsSeenAsync(::IUnknown* sourceIds, ::IUnknown** result) = 0;
};};

template <> struct abi<Windows::ApplicationModel::Calls::IVoipCallCoordinator>{ struct type : ::IInspectable
{
    virtual HRESULT __stdcall ReserveCallResourcesAsync(HSTRING taskEntryPoint, ::IUnknown** operation) = 0;
    virtual HRESULT __stdcall add_MuteStateChanged(::IUnknown* muteChangeHandler, event_token* token) = 0;
    virtual HRESULT __stdcall remove_MuteStateChanged(event_token token) = 0;
    virtual HRESULT __stdcall RequestNewIncomingCall(HSTRING context, HSTRING contactName, HSTRING contactNumber, ::IUnknown* contactImage, HSTRING serviceName, ::IUnknown* brandingImage, HSTRING callDetails, ::IUnknown* ringtone, Windows::ApplicationModel::Calls::VoipPhoneCallMedia media, Windows::Foundation::TimeSpan ringTimeout, ::IUnknown** call) = 0;
    virtual HRESULT __stdcall RequestNewOutgoingCall(HSTRING context, HSTRING contactName, HSTRING serviceName, Windows::ApplicationModel::Calls::VoipPhoneCallMedia media, ::IUnknown** call) = 0;
    virtual HRESULT __stdcall NotifyMuted() = 0;
    virtual HRESULT __stdcall NotifyUnmuted() = 0;
    virtual HRESULT __stdcall RequestOutgoingUpgradeToVideoCall(GUID callUpgradeGuid, HSTRING context, HSTRING contactName, HSTRING serviceName, ::IUnknown** call) = 0;
    virtual HRESULT __stdcall RequestIncomingUpgradeToVideoCall(HSTRING context, HSTRING contactName, HSTRING contactNumber, ::IUnknown* contactImage, HSTRING serviceName, ::IUnknown* brandingImage, HSTRING callDetails, ::IUnknown* ringtone, Windows::Foundation::TimeSpan ringTimeout, ::IUnknown** call) = 0;
    virtual HRESULT __stdcall TerminateCellularCall(GUID callUpgradeGuid) = 0;
    virtual HRESULT __stdcall CancelUpgrade(GUID callUpgradeGuid) = 0;
};};

template <> struct abi<Windows::ApplicationModel::Calls::IVoipCallCoordinator2>{ struct type : ::IInspectable
{
    virtual HRESULT __stdcall SetupNewAcceptedCall(HSTRING context, HSTRING contactName, HSTRING contactNumber, HSTRING serviceName, Windows::ApplicationModel::Calls::VoipPhoneCallMedia media, ::IUnknown** call) = 0;
};};

template <> struct abi<Windows::ApplicationModel::Calls::IVoipCallCoordinatorStatics>{ struct type : ::IInspectable
{
    virtual HRESULT __stdcall GetDefault(::IUnknown** coordinator) = 0;
};};

template <> struct abi<Windows::ApplicationModel::Calls::IVoipPhoneCall>{ struct type : ::IInspectable
{
    virtual HRESULT __stdcall add_EndRequested(::IUnknown* handler, event_token* token) = 0;
    virtual HRESULT __stdcall remove_EndRequested(event_token token) = 0;
    virtual HRESULT __stdcall add_HoldRequested(::IUnknown* handler, event_token* token) = 0;
    virtual HRESULT __stdcall remove_HoldRequested(event_token token) = 0;
    virtual HRESULT __stdcall add_ResumeRequested(::IUnknown* handler, event_token* token) = 0;
    virtual HRESULT __stdcall remove_ResumeRequested(event_token token) = 0;
    virtual HRESULT __stdcall add_AnswerRequested(::IUnknown* acceptHandler, event_token* token) = 0;
    virtual HRESULT __stdcall remove_AnswerRequested(event_token token) = 0;
    virtual HRESULT __stdcall add_RejectRequested(::IUnknown* rejectHandler, event_token* token) = 0;
    virtual HRESULT __stdcall remove_RejectRequested(event_token token) = 0;
    virtual HRESULT __stdcall NotifyCallHeld() = 0;
    virtual HRESULT __stdcall NotifyCallActive() = 0;
    virtual HRESULT __stdcall NotifyCallEnded() = 0;
    virtual HRESULT __stdcall get_ContactName(HSTRING* value) = 0;
    virtual HRESULT __stdcall put_ContactName(HSTRING value) = 0;
    virtual HRESULT __stdcall get_StartTime(Windows::Foundation::DateTime* value) = 0;
    virtual HRESULT __stdcall put_StartTime(Windows::Foundation::DateTime value) = 0;
    virtual HRESULT __stdcall get_CallMedia(Windows::ApplicationModel::Calls::VoipPhoneCallMedia* value) = 0;
    virtual HRESULT __stdcall put_CallMedia(Windows::ApplicationModel::Calls::VoipPhoneCallMedia value) = 0;
    virtual HRESULT __stdcall NotifyCallReady() = 0;
};};

template <> struct abi<Windows::ApplicationModel::Calls::IVoipPhoneCall2>{ struct type : ::IInspectable
{
    virtual HRESULT __stdcall TryShowAppUI() = 0;
};};

}
