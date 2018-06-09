/* Header file automatically generated from Microsoft.UI.Composition.Toolkit.idl */
/*
 * File built with Microsoft(R) MIDLRT Compiler Engine Version 10.00.0206 
 */

#pragma warning( disable: 4049 )  /* more than 64k source lines */

/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 500
#endif

/* verify that the <rpcsal.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCSAL_H_VERSION__
#define __REQUIRED_RPCSAL_H_VERSION__ 100
#endif

#include <rpc.h>
#include <rpcndr.h>

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif /* __RPCNDR_H_VERSION__ */

#ifndef COM_NO_WINDOWS_H
#include <windows.h>
#include <ole2.h>
#endif /*COM_NO_WINDOWS_H*/
#ifndef __Microsoft2EUI2EComposition2EToolkit_h_h__
#define __Microsoft2EUI2EComposition2EToolkit_h_h__
#ifndef __Microsoft2EUI2EComposition2EToolkit_h_p_h__
#define __Microsoft2EUI2EComposition2EToolkit_h_p_h__


#pragma once

#pragma push_macro("MIDL_CONST_ID")
#if !defined(_MSC_VER) || (_MSC_VER >= 1910)
#define MIDL_CONST_ID constexpr const
#else
#define MIDL_CONST_ID const __declspec(selectany)
#endif


//  API Contract Inclusion Definitions
#if !defined(SPECIFIC_API_CONTRACT_DEFINITIONS)
#if !defined(WINDOWS_APPLICATIONMODEL_ACTIVATION_ACTIVATEDEVENTSCONTRACT_VERSION)
#define WINDOWS_APPLICATIONMODEL_ACTIVATION_ACTIVATEDEVENTSCONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_APPLICATIONMODEL_ACTIVATION_ACTIVATEDEVENTSCONTRACT_VERSION)

#if !defined(WINDOWS_APPLICATIONMODEL_ACTIVATION_ACTIVATIONCAMERASETTINGSCONTRACT_VERSION)
#define WINDOWS_APPLICATIONMODEL_ACTIVATION_ACTIVATIONCAMERASETTINGSCONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_APPLICATIONMODEL_ACTIVATION_ACTIVATIONCAMERASETTINGSCONTRACT_VERSION)

#if !defined(WINDOWS_APPLICATIONMODEL_ACTIVATION_CONTACTACTIVATEDEVENTSCONTRACT_VERSION)
#define WINDOWS_APPLICATIONMODEL_ACTIVATION_CONTACTACTIVATEDEVENTSCONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_APPLICATIONMODEL_ACTIVATION_CONTACTACTIVATEDEVENTSCONTRACT_VERSION)

#if !defined(WINDOWS_APPLICATIONMODEL_ACTIVATION_WEBUISEARCHACTIVATEDEVENTSCONTRACT_VERSION)
#define WINDOWS_APPLICATIONMODEL_ACTIVATION_WEBUISEARCHACTIVATEDEVENTSCONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_APPLICATIONMODEL_ACTIVATION_WEBUISEARCHACTIVATEDEVENTSCONTRACT_VERSION)

#if !defined(WINDOWS_APPLICATIONMODEL_BACKGROUND_BACKGROUNDALARMAPPLICATIONCONTRACT_VERSION)
#define WINDOWS_APPLICATIONMODEL_BACKGROUND_BACKGROUNDALARMAPPLICATIONCONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_APPLICATIONMODEL_BACKGROUND_BACKGROUNDALARMAPPLICATIONCONTRACT_VERSION)

#if !defined(WINDOWS_APPLICATIONMODEL_CALLS_BACKGROUND_CALLSBACKGROUNDCONTRACT_VERSION)
#define WINDOWS_APPLICATIONMODEL_CALLS_BACKGROUND_CALLSBACKGROUNDCONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_APPLICATIONMODEL_CALLS_BACKGROUND_CALLSBACKGROUNDCONTRACT_VERSION)

#if !defined(WINDOWS_APPLICATIONMODEL_CALLS_CALLSPHONECONTRACT_VERSION)
#define WINDOWS_APPLICATIONMODEL_CALLS_CALLSPHONECONTRACT_VERSION 0x30000
#endif // defined(WINDOWS_APPLICATIONMODEL_CALLS_CALLSPHONECONTRACT_VERSION)

#if !defined(WINDOWS_APPLICATIONMODEL_CALLS_CALLSVOIPCONTRACT_VERSION)
#define WINDOWS_APPLICATIONMODEL_CALLS_CALLSVOIPCONTRACT_VERSION 0x20000
#endif // defined(WINDOWS_APPLICATIONMODEL_CALLS_CALLSVOIPCONTRACT_VERSION)

#if !defined(WINDOWS_APPLICATIONMODEL_CALLS_LOCKSCREENCALLCONTRACT_VERSION)
#define WINDOWS_APPLICATIONMODEL_CALLS_LOCKSCREENCALLCONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_APPLICATIONMODEL_CALLS_LOCKSCREENCALLCONTRACT_VERSION)

#if !defined(WINDOWS_APPLICATIONMODEL_FULLTRUSTAPPCONTRACT_VERSION)
#define WINDOWS_APPLICATIONMODEL_FULLTRUSTAPPCONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_APPLICATIONMODEL_FULLTRUSTAPPCONTRACT_VERSION)

#if !defined(WINDOWS_APPLICATIONMODEL_SEARCH_SEARCHCONTRACT_VERSION)
#define WINDOWS_APPLICATIONMODEL_SEARCH_SEARCHCONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_APPLICATIONMODEL_SEARCH_SEARCHCONTRACT_VERSION)

#if !defined(WINDOWS_APPLICATIONMODEL_STARTUPTASKCONTRACT_VERSION)
#define WINDOWS_APPLICATIONMODEL_STARTUPTASKCONTRACT_VERSION 0x20000
#endif // defined(WINDOWS_APPLICATIONMODEL_STARTUPTASKCONTRACT_VERSION)

#if !defined(WINDOWS_APPLICATIONMODEL_WALLET_WALLETCONTRACT_VERSION)
#define WINDOWS_APPLICATIONMODEL_WALLET_WALLETCONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_APPLICATIONMODEL_WALLET_WALLETCONTRACT_VERSION)

#if !defined(WINDOWS_DEVICES_PRINTERS_EXTENSIONS_EXTENSIONSCONTRACT_VERSION)
#define WINDOWS_DEVICES_PRINTERS_EXTENSIONS_EXTENSIONSCONTRACT_VERSION 0x20000
#endif // defined(WINDOWS_DEVICES_PRINTERS_EXTENSIONS_EXTENSIONSCONTRACT_VERSION)

#if !defined(WINDOWS_DEVICES_SMARTCARDS_SMARTCARDBACKGROUNDTRIGGERCONTRACT_VERSION)
#define WINDOWS_DEVICES_SMARTCARDS_SMARTCARDBACKGROUNDTRIGGERCONTRACT_VERSION 0x30000
#endif // defined(WINDOWS_DEVICES_SMARTCARDS_SMARTCARDBACKGROUNDTRIGGERCONTRACT_VERSION)

#if !defined(WINDOWS_DEVICES_SMARTCARDS_SMARTCARDEMULATORCONTRACT_VERSION)
#define WINDOWS_DEVICES_SMARTCARDS_SMARTCARDEMULATORCONTRACT_VERSION 0x50000
#endif // defined(WINDOWS_DEVICES_SMARTCARDS_SMARTCARDEMULATORCONTRACT_VERSION)

#if !defined(WINDOWS_DEVICES_SMS_LEGACYSMSAPICONTRACT_VERSION)
#define WINDOWS_DEVICES_SMS_LEGACYSMSAPICONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_DEVICES_SMS_LEGACYSMSAPICONTRACT_VERSION)

#if !defined(WINDOWS_FOUNDATION_FOUNDATIONCONTRACT_VERSION)
#define WINDOWS_FOUNDATION_FOUNDATIONCONTRACT_VERSION 0x30000
#endif // defined(WINDOWS_FOUNDATION_FOUNDATIONCONTRACT_VERSION)

#if !defined(WINDOWS_FOUNDATION_UNIVERSALAPICONTRACT_VERSION)
#define WINDOWS_FOUNDATION_UNIVERSALAPICONTRACT_VERSION 0x50000
#endif // defined(WINDOWS_FOUNDATION_UNIVERSALAPICONTRACT_VERSION)

#if !defined(WINDOWS_GAMING_INPUT_GAMINGINPUTPREVIEWCONTRACT_VERSION)
#define WINDOWS_GAMING_INPUT_GAMINGINPUTPREVIEWCONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_GAMING_INPUT_GAMINGINPUTPREVIEWCONTRACT_VERSION)

#if !defined(WINDOWS_GLOBALIZATION_GLOBALIZATIONJAPANESEPHONETICANALYZERCONTRACT_VERSION)
#define WINDOWS_GLOBALIZATION_GLOBALIZATIONJAPANESEPHONETICANALYZERCONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_GLOBALIZATION_GLOBALIZATIONJAPANESEPHONETICANALYZERCONTRACT_VERSION)

#if !defined(WINDOWS_MEDIA_CAPTURE_APPBROADCASTCONTRACT_VERSION)
#define WINDOWS_MEDIA_CAPTURE_APPBROADCASTCONTRACT_VERSION 0x20000
#endif // defined(WINDOWS_MEDIA_CAPTURE_APPBROADCASTCONTRACT_VERSION)

#if !defined(WINDOWS_MEDIA_CAPTURE_APPCAPTURECONTRACT_VERSION)
#define WINDOWS_MEDIA_CAPTURE_APPCAPTURECONTRACT_VERSION 0x40000
#endif // defined(WINDOWS_MEDIA_CAPTURE_APPCAPTURECONTRACT_VERSION)

#if !defined(WINDOWS_MEDIA_CAPTURE_APPCAPTUREMETADATACONTRACT_VERSION)
#define WINDOWS_MEDIA_CAPTURE_APPCAPTUREMETADATACONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_MEDIA_CAPTURE_APPCAPTUREMETADATACONTRACT_VERSION)

#if !defined(WINDOWS_MEDIA_CAPTURE_CAMERACAPTUREUICONTRACT_VERSION)
#define WINDOWS_MEDIA_CAPTURE_CAMERACAPTUREUICONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_MEDIA_CAPTURE_CAMERACAPTUREUICONTRACT_VERSION)

#if !defined(WINDOWS_MEDIA_CAPTURE_GAMEBARCONTRACT_VERSION)
#define WINDOWS_MEDIA_CAPTURE_GAMEBARCONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_MEDIA_CAPTURE_GAMEBARCONTRACT_VERSION)

#if !defined(WINDOWS_MEDIA_DEVICES_CALLCONTROLCONTRACT_VERSION)
#define WINDOWS_MEDIA_DEVICES_CALLCONTROLCONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_MEDIA_DEVICES_CALLCONTROLCONTRACT_VERSION)

#if !defined(WINDOWS_MEDIA_MEDIACONTROLCONTRACT_VERSION)
#define WINDOWS_MEDIA_MEDIACONTROLCONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_MEDIA_MEDIACONTROLCONTRACT_VERSION)

#if !defined(WINDOWS_MEDIA_PROTECTION_PROTECTIONRENEWALCONTRACT_VERSION)
#define WINDOWS_MEDIA_PROTECTION_PROTECTIONRENEWALCONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_MEDIA_PROTECTION_PROTECTIONRENEWALCONTRACT_VERSION)

#if !defined(WINDOWS_NETWORKING_CONNECTIVITY_WWANCONTRACT_VERSION)
#define WINDOWS_NETWORKING_CONNECTIVITY_WWANCONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_NETWORKING_CONNECTIVITY_WWANCONTRACT_VERSION)

#if !defined(WINDOWS_NETWORKING_SOCKETS_CONTROLCHANNELTRIGGERCONTRACT_VERSION)
#define WINDOWS_NETWORKING_SOCKETS_CONTROLCHANNELTRIGGERCONTRACT_VERSION 0x20000
#endif // defined(WINDOWS_NETWORKING_SOCKETS_CONTROLCHANNELTRIGGERCONTRACT_VERSION)

#if !defined(WINDOWS_PHONE_PHONECONTRACT_VERSION)
#define WINDOWS_PHONE_PHONECONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_PHONE_PHONECONTRACT_VERSION)

#if !defined(WINDOWS_PHONE_PHONEINTERNALCONTRACT_VERSION)
#define WINDOWS_PHONE_PHONEINTERNALCONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_PHONE_PHONEINTERNALCONTRACT_VERSION)

#if !defined(WINDOWS_SECURITY_ENTERPRISEDATA_ENTERPRISEDATACONTRACT_VERSION)
#define WINDOWS_SECURITY_ENTERPRISEDATA_ENTERPRISEDATACONTRACT_VERSION 0x50000
#endif // defined(WINDOWS_SECURITY_ENTERPRISEDATA_ENTERPRISEDATACONTRACT_VERSION)

#if !defined(WINDOWS_STORAGE_PROVIDER_CLOUDFILESCONTRACT_VERSION)
#define WINDOWS_STORAGE_PROVIDER_CLOUDFILESCONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_STORAGE_PROVIDER_CLOUDFILESCONTRACT_VERSION)

#if !defined(WINDOWS_SYSTEM_SYSTEMMANAGEMENTCONTRACT_VERSION)
#define WINDOWS_SYSTEM_SYSTEMMANAGEMENTCONTRACT_VERSION 0x40000
#endif // defined(WINDOWS_SYSTEM_SYSTEMMANAGEMENTCONTRACT_VERSION)

#if !defined(WINDOWS_UI_CORE_COREWINDOWDIALOGSCONTRACT_VERSION)
#define WINDOWS_UI_CORE_COREWINDOWDIALOGSCONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_UI_CORE_COREWINDOWDIALOGSCONTRACT_VERSION)

#if !defined(WINDOWS_UI_VIEWMANAGEMENT_VIEWMANAGEMENTVIEWSCALINGCONTRACT_VERSION)
#define WINDOWS_UI_VIEWMANAGEMENT_VIEWMANAGEMENTVIEWSCALINGCONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_UI_VIEWMANAGEMENT_VIEWMANAGEMENTVIEWSCALINGCONTRACT_VERSION)

#if !defined(WINDOWS_UI_WEBUI_CORE_WEBUICOMMANDBARCONTRACT_VERSION)
#define WINDOWS_UI_WEBUI_CORE_WEBUICOMMANDBARCONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_UI_WEBUI_CORE_WEBUICOMMANDBARCONTRACT_VERSION)

#endif // defined(SPECIFIC_API_CONTRACT_DEFINITIONS)


// Header files for imported files
#include "inspectable.h"
#include "AsyncInfo.h"
#include "EventToken.h"
#include "Windows.Foundation.h"
#include "Windows.Graphics.DirectX.h"
#include "Windows.Storage.h"
#include "Windows.UI.Composition.h"

#if defined(__cplusplus) && !defined(CINTERFACE)
/* Forward Declarations */
#ifndef ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandler_FWD_DEFINED__
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandler_FWD_DEFINED__

namespace Microsoft {
    namespace UI {
        namespace Composition {
            namespace Toolkit {
                interface ICompositionGraphicsDeviceLostEventHandler;
            } /* Microsoft */
        } /* UI */
    } /* Composition */
} /* Toolkit */
#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandler Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDeviceLostEventHandler

#endif // ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandler_FWD_DEFINED__

#ifndef ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandler_FWD_DEFINED__
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandler_FWD_DEFINED__

namespace Microsoft {
    namespace UI {
        namespace Composition {
            namespace Toolkit {
                interface ICompositionImageLoadedEventHandler;
            } /* Microsoft */
        } /* UI */
    } /* Composition */
} /* Toolkit */
#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandler Microsoft::UI::Composition::Toolkit::ICompositionImageLoadedEventHandler

#endif // ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandler_FWD_DEFINED__

#ifndef ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice_FWD_DEFINED__
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice_FWD_DEFINED__

namespace Microsoft {
    namespace UI {
        namespace Composition {
            namespace Toolkit {
                interface ICompositionGraphicsDevice;
            } /* Microsoft */
        } /* UI */
    } /* Composition */
} /* Toolkit */
#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDevice

#endif // ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice_FWD_DEFINED__

#ifndef ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics_FWD_DEFINED__
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics_FWD_DEFINED__

namespace Microsoft {
    namespace UI {
        namespace Composition {
            namespace Toolkit {
                interface ICompositionGraphicsDeviceStatics;
            } /* Microsoft */
        } /* UI */
    } /* Composition */
} /* Toolkit */
#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDeviceStatics

#endif // ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics_FWD_DEFINED__

#ifndef ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory_FWD_DEFINED__
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory_FWD_DEFINED__

namespace Microsoft {
    namespace UI {
        namespace Composition {
            namespace Toolkit {
                interface ICompositionImageFactory;
            } /* Microsoft */
        } /* UI */
    } /* Composition */
} /* Toolkit */
#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory Microsoft::UI::Composition::Toolkit::ICompositionImageFactory

#endif // ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory_FWD_DEFINED__

#ifndef ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics_FWD_DEFINED__
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics_FWD_DEFINED__

namespace Microsoft {
    namespace UI {
        namespace Composition {
            namespace Toolkit {
                interface ICompositionImageFactoryStatics;
            } /* Microsoft */
        } /* UI */
    } /* Composition */
} /* Toolkit */
#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics Microsoft::UI::Composition::Toolkit::ICompositionImageFactoryStatics

#endif // ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics_FWD_DEFINED__

#ifndef ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions_FWD_DEFINED__
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions_FWD_DEFINED__

namespace Microsoft {
    namespace UI {
        namespace Composition {
            namespace Toolkit {
                interface ICompositionImageOptions;
            } /* Microsoft */
        } /* UI */
    } /* Composition */
} /* Toolkit */
#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions Microsoft::UI::Composition::Toolkit::ICompositionImageOptions

#endif // ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions_FWD_DEFINED__

#ifndef ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage_FWD_DEFINED__
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage_FWD_DEFINED__

namespace Microsoft {
    namespace UI {
        namespace Composition {
            namespace Toolkit {
                interface ICompositionImage;
            } /* Microsoft */
        } /* UI */
    } /* Composition */
} /* Toolkit */
#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage Microsoft::UI::Composition::Toolkit::ICompositionImage

#endif // ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage_FWD_DEFINED__











namespace Microsoft {
    namespace UI {
        namespace Composition {
            namespace Toolkit {
                class CompositionGraphicsDevice;
            } /* Microsoft */
        } /* UI */
    } /* Composition */
} /* Toolkit */



namespace Microsoft {
    namespace UI {
        namespace Composition {
            namespace Toolkit {
                class CompositionImage;
            } /* Microsoft */
        } /* UI */
    } /* Composition */
} /* Toolkit */



namespace Microsoft {
    namespace UI {
        namespace Composition {
            namespace Toolkit {
                class CompositionImageFactory;
            } /* Microsoft */
        } /* UI */
    } /* Composition */
} /* Toolkit */



namespace Microsoft {
    namespace UI {
        namespace Composition {
            namespace Toolkit {
                class CompositionImageOptions;
            } /* Microsoft */
        } /* UI */
    } /* Composition */
} /* Toolkit */



namespace Microsoft {
    namespace UI {
        namespace Composition {
            namespace Toolkit {
                
                typedef enum CompositionImageLoadStatus : int CompositionImageLoadStatus;
                
            } /* Microsoft */
        } /* UI */
    } /* Composition */
} /* Toolkit */


/*
 *
 * Struct Microsoft.UI.Composition.Toolkit.CompositionImageLoadStatus
 *
 */


namespace Microsoft {
    namespace UI {
        namespace Composition {
            namespace Toolkit {
                /* [v1_enum, version] */
                enum CompositionImageLoadStatus : int
                {
                    CompositionImageLoadStatus_Success = 0,
                    CompositionImageLoadStatus_FileAccessError = 1,
                    CompositionImageLoadStatus_DecodeError = 2,
                    CompositionImageLoadStatus_NotEnoughResources = 3,
                    CompositionImageLoadStatus_Other = 4,
                };
                
            } /* Microsoft */
        } /* UI */
    } /* Composition */
} /* Toolkit */


/*
 *
 * Delegate Microsoft.UI.Composition.Toolkit.CompositionGraphicsDeviceLostEventHandler
 *
 */
#if !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandler_INTERFACE_DEFINED__)
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandler_INTERFACE_DEFINED__

namespace Microsoft {
    namespace UI {
        namespace Composition {
            namespace Toolkit {
                /* [object, version, uuid("0BE20CB6-FE18-3023-9680-F4AA2108BCEE")] */
                MIDL_INTERFACE("0BE20CB6-FE18-3023-9680-F4AA2108BCEE")
                ICompositionGraphicsDeviceLostEventHandler : IUnknown
                {
                    virtual HRESULT STDMETHODCALLTYPE Invoke(
                        /* [in] */Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDevice * sender
                        ) = 0;
                    
                };

                extern MIDL_CONST_ID IID & IID_ICompositionGraphicsDeviceLostEventHandler=_uuidof(ICompositionGraphicsDeviceLostEventHandler);
                
            } /* Microsoft */
        } /* UI */
    } /* Composition */
} /* Toolkit */

EXTERN_C const IID IID___x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandler;
#endif /* !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandler_INTERFACE_DEFINED__) */


/*
 *
 * Delegate Microsoft.UI.Composition.Toolkit.CompositionImageLoadedEventHandler
 *
 */
#if !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandler_INTERFACE_DEFINED__)
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandler_INTERFACE_DEFINED__

namespace Microsoft {
    namespace UI {
        namespace Composition {
            namespace Toolkit {
                /* [object, version, uuid("5B3AA813-CEBF-38B9-8F31-D23EB8C24F86")] */
                MIDL_INTERFACE("5B3AA813-CEBF-38B9-8F31-D23EB8C24F86")
                ICompositionImageLoadedEventHandler : IUnknown
                {
                    virtual HRESULT STDMETHODCALLTYPE Invoke(
                        /* [in] */Microsoft::UI::Composition::Toolkit::ICompositionImage * sender,
                        /* [in] */Microsoft::UI::Composition::Toolkit::CompositionImageLoadStatus status
                        ) = 0;
                    
                };

                extern MIDL_CONST_ID IID & IID_ICompositionImageLoadedEventHandler=_uuidof(ICompositionImageLoadedEventHandler);
                
            } /* Microsoft */
        } /* UI */
    } /* Composition */
} /* Toolkit */

EXTERN_C const IID IID___x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandler;
#endif /* !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandler_INTERFACE_DEFINED__) */


/*
 *
 * Interface Microsoft.UI.Composition.Toolkit.ICompositionGraphicsDevice
 *
 * Interface is a part of the implementation of type Microsoft.UI.Composition.Toolkit.CompositionGraphicsDevice
 *
 *
 */
#if !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice_INTERFACE_DEFINED__)
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Microsoft_UI_Composition_Toolkit_ICompositionGraphicsDevice[] = L"Microsoft.UI.Composition.Toolkit.ICompositionGraphicsDevice";

namespace Microsoft {
    namespace UI {
        namespace Composition {
            namespace Toolkit {
                /* [object, version, uuid("4A0E578E-68FA-3394-905A-D4D9D8AE30CA"), exclusiveto] */
                MIDL_INTERFACE("4A0E578E-68FA-3394-905A-D4D9D8AE30CA")
                ICompositionGraphicsDevice : IInspectable
                {
                    /* [eventadd] */virtual HRESULT STDMETHODCALLTYPE add_DeviceLost(
                        /* [in] */Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDeviceLostEventHandler  * handler,
                        /* [retval, out] */EventRegistrationToken * token
                        ) = 0;
                    /* [eventremove] */virtual HRESULT STDMETHODCALLTYPE remove_DeviceLost(
                        /* [in] */EventRegistrationToken token
                        ) = 0;
                    virtual HRESULT STDMETHODCALLTYPE CreateDrawingSurface(
                        /* [in] */Windows::Foundation::Size sizePixels,
                        /* [in] */Windows::Graphics::DirectX::DirectXPixelFormat pixelFormat,
                        /* [in] */Windows::Graphics::DirectX::DirectXAlphaMode alphaMode,
                        /* [retval, out] */Windows::UI::Composition::ICompositionSurface * * surface
                        ) = 0;
                    virtual HRESULT STDMETHODCALLTYPE AcquireDrawingLock(void) = 0;
                    virtual HRESULT STDMETHODCALLTYPE ReleaseDrawingLock(void) = 0;
                    
                };

                extern MIDL_CONST_ID IID & IID_ICompositionGraphicsDevice=_uuidof(ICompositionGraphicsDevice);
                
            } /* Microsoft */
        } /* UI */
    } /* Composition */
} /* Toolkit */

EXTERN_C const IID IID___x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice;
#endif /* !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice_INTERFACE_DEFINED__) */


/*
 *
 * Interface Microsoft.UI.Composition.Toolkit.ICompositionGraphicsDeviceStatics
 *
 * Interface is a part of the implementation of type Microsoft.UI.Composition.Toolkit.CompositionGraphicsDevice
 *
 *
 */
#if !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics_INTERFACE_DEFINED__)
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Microsoft_UI_Composition_Toolkit_ICompositionGraphicsDeviceStatics[] = L"Microsoft.UI.Composition.Toolkit.ICompositionGraphicsDeviceStatics";

namespace Microsoft {
    namespace UI {
        namespace Composition {
            namespace Toolkit {
                /* [object, version, uuid("BC22813B-C736-3176-9D45-75F86BEAE8B6"), exclusiveto] */
                MIDL_INTERFACE("BC22813B-C736-3176-9D45-75F86BEAE8B6")
                ICompositionGraphicsDeviceStatics : IInspectable
                {
                    virtual HRESULT STDMETHODCALLTYPE CreateCompositionGraphicsDevice(
                        /* [in] */Windows::UI::Composition::ICompositor * compositor,
                        /* [retval, out] */Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDevice * * device
                        ) = 0;
                    
                };

                extern MIDL_CONST_ID IID & IID_ICompositionGraphicsDeviceStatics=_uuidof(ICompositionGraphicsDeviceStatics);
                
            } /* Microsoft */
        } /* UI */
    } /* Composition */
} /* Toolkit */

EXTERN_C const IID IID___x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics;
#endif /* !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics_INTERFACE_DEFINED__) */


/*
 *
 * Interface Microsoft.UI.Composition.Toolkit.ICompositionImageFactory
 *
 * Interface is a part of the implementation of type Microsoft.UI.Composition.Toolkit.CompositionImageFactory
 *
 *
 */
#if !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory_INTERFACE_DEFINED__)
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Microsoft_UI_Composition_Toolkit_ICompositionImageFactory[] = L"Microsoft.UI.Composition.Toolkit.ICompositionImageFactory";

namespace Microsoft {
    namespace UI {
        namespace Composition {
            namespace Toolkit {
                /* [object, version, uuid("25372FEC-3FA1-3C03-980F-FC48BA44680F"), exclusiveto] */
                MIDL_INTERFACE("25372FEC-3FA1-3C03-980F-FC48BA44680F")
                ICompositionImageFactory : IInspectable
                {
                    /* [overload] */virtual HRESULT STDMETHODCALLTYPE CreateImageFromUri2(
                        /* [in] */Windows::Foundation::IUriRuntimeClass * uri,
                        /* [retval, out] */Microsoft::UI::Composition::Toolkit::ICompositionImage * * image
                        ) = 0;
                    /* [overload] */virtual HRESULT STDMETHODCALLTYPE CreateImageFromUri1(
                        /* [in] */Windows::Foundation::IUriRuntimeClass * uri,
                        /* [in] */Microsoft::UI::Composition::Toolkit::ICompositionImageOptions * options,
                        /* [retval, out] */Microsoft::UI::Composition::Toolkit::ICompositionImage * * image
                        ) = 0;
                    /* [overload] */virtual HRESULT STDMETHODCALLTYPE CreateImageFromFile2(
                        /* [in] */Windows::Storage::IStorageFile * file,
                        /* [retval, out] */Microsoft::UI::Composition::Toolkit::ICompositionImage * * image
                        ) = 0;
                    /* [overload] */virtual HRESULT STDMETHODCALLTYPE CreateImageFromFile1(
                        /* [in] */Windows::Storage::IStorageFile * file,
                        /* [in] */Microsoft::UI::Composition::Toolkit::ICompositionImageOptions * options,
                        /* [retval, out] */Microsoft::UI::Composition::Toolkit::ICompositionImage * * image
                        ) = 0;
                    virtual HRESULT STDMETHODCALLTYPE CreateImageFromPixels(
                        /* [in] */UINT32 pixelsSize,
                        /* [size_is(pixelsSize), in] */BYTE * pixels,
                        /* [in] */INT32 pixelWidth,
                        /* [in] */INT32 pixelHeight,
                        /* [retval, out] */Microsoft::UI::Composition::Toolkit::ICompositionImage * * image
                        ) = 0;
                    
                };

                extern MIDL_CONST_ID IID & IID_ICompositionImageFactory=_uuidof(ICompositionImageFactory);
                
            } /* Microsoft */
        } /* UI */
    } /* Composition */
} /* Toolkit */

EXTERN_C const IID IID___x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory;
#endif /* !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory_INTERFACE_DEFINED__) */


/*
 *
 * Interface Microsoft.UI.Composition.Toolkit.ICompositionImageFactoryStatics
 *
 * Interface is a part of the implementation of type Microsoft.UI.Composition.Toolkit.CompositionImageFactory
 *
 *
 */
#if !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics_INTERFACE_DEFINED__)
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Microsoft_UI_Composition_Toolkit_ICompositionImageFactoryStatics[] = L"Microsoft.UI.Composition.Toolkit.ICompositionImageFactoryStatics";

namespace Microsoft {
    namespace UI {
        namespace Composition {
            namespace Toolkit {
                /* [object, version, uuid("EB167AED-6BF0-39AE-90B1-7C05FF0F308F"), exclusiveto] */
                MIDL_INTERFACE("EB167AED-6BF0-39AE-90B1-7C05FF0F308F")
                ICompositionImageFactoryStatics : IInspectable
                {
                    virtual HRESULT STDMETHODCALLTYPE CreateCompositionImageFactory(
                        /* [in] */Windows::UI::Composition::ICompositor * compositor,
                        /* [retval, out] */Microsoft::UI::Composition::Toolkit::ICompositionImageFactory * * factory
                        ) = 0;
                    
                };

                extern MIDL_CONST_ID IID & IID_ICompositionImageFactoryStatics=_uuidof(ICompositionImageFactoryStatics);
                
            } /* Microsoft */
        } /* UI */
    } /* Composition */
} /* Toolkit */

EXTERN_C const IID IID___x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics;
#endif /* !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics_INTERFACE_DEFINED__) */


/*
 *
 * Interface Microsoft.UI.Composition.Toolkit.ICompositionImageOptions
 *
 * Interface is a part of the implementation of type Microsoft.UI.Composition.Toolkit.CompositionImageOptions
 *
 *
 */
#if !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions_INTERFACE_DEFINED__)
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Microsoft_UI_Composition_Toolkit_ICompositionImageOptions[] = L"Microsoft.UI.Composition.Toolkit.ICompositionImageOptions";

namespace Microsoft {
    namespace UI {
        namespace Composition {
            namespace Toolkit {
                /* [object, version, uuid("8554C61C-F17B-3602-868B-C17C9F7BC449"), exclusiveto] */
                MIDL_INTERFACE("8554C61C-F17B-3602-868B-C17C9F7BC449")
                ICompositionImageOptions : IInspectable
                {
                    /* [propget] */virtual HRESULT STDMETHODCALLTYPE get_DecodeWidth(
                        /* [retval, out] */INT32 * value
                        ) = 0;
                    /* [propput] */virtual HRESULT STDMETHODCALLTYPE put_DecodeWidth(
                        /* [in] */INT32 value
                        ) = 0;
                    /* [propget] */virtual HRESULT STDMETHODCALLTYPE get_DecodeHeight(
                        /* [retval, out] */INT32 * value
                        ) = 0;
                    /* [propput] */virtual HRESULT STDMETHODCALLTYPE put_DecodeHeight(
                        /* [in] */INT32 value
                        ) = 0;
                    
                };

                extern MIDL_CONST_ID IID & IID_ICompositionImageOptions=_uuidof(ICompositionImageOptions);
                
            } /* Microsoft */
        } /* UI */
    } /* Composition */
} /* Toolkit */

EXTERN_C const IID IID___x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions;
#endif /* !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions_INTERFACE_DEFINED__) */


/*
 *
 * Interface Microsoft.UI.Composition.Toolkit.ICompositionImage
 *
 * Interface is a part of the implementation of type Microsoft.UI.Composition.Toolkit.CompositionImage
 *
 *
 */
#if !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage_INTERFACE_DEFINED__)
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Microsoft_UI_Composition_Toolkit_ICompositionImage[] = L"Microsoft.UI.Composition.Toolkit.ICompositionImage";

namespace Microsoft {
    namespace UI {
        namespace Composition {
            namespace Toolkit {
                /* [object, version, uuid("D5E4B2E9-82F7-3A11-8D7A-E58B892E054C"), exclusiveto] */
                MIDL_INTERFACE("D5E4B2E9-82F7-3A11-8D7A-E58B892E054C")
                ICompositionImage : IInspectable
                {
                    /* [eventadd] */virtual HRESULT STDMETHODCALLTYPE add_ImageLoaded(
                        /* [in] */Microsoft::UI::Composition::Toolkit::ICompositionImageLoadedEventHandler  * handler,
                        /* [retval, out] */EventRegistrationToken * token
                        ) = 0;
                    /* [eventremove] */virtual HRESULT STDMETHODCALLTYPE remove_ImageLoaded(
                        /* [in] */EventRegistrationToken token
                        ) = 0;
                    /* [propget] */virtual HRESULT STDMETHODCALLTYPE get_Size(
                        /* [retval, out] */Windows::Foundation::Size * value
                        ) = 0;
                    /* [propget] */virtual HRESULT STDMETHODCALLTYPE get_Surface(
                        /* [retval, out] */Windows::UI::Composition::ICompositionSurface * * value
                        ) = 0;
                    
                };

                extern MIDL_CONST_ID IID & IID_ICompositionImage=_uuidof(ICompositionImage);
                
            } /* Microsoft */
        } /* UI */
    } /* Composition */
} /* Toolkit */

EXTERN_C const IID IID___x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage;
#endif /* !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage_INTERFACE_DEFINED__) */


/*
 *
 * Class Microsoft.UI.Composition.Toolkit.CompositionGraphicsDevice
 *
 * RuntimeClass contains static methods.
 *
 * Class implements the following interfaces:
 *    Microsoft.UI.Composition.Toolkit.ICompositionGraphicsDevice ** Default Interface **
 *    Windows.Foundation.IClosable
 *
 * Class Threading Model:  Both Single and Multi Threaded Apartment
 *
 * Class Marshaling Behavior:  Agile - Class is agile
 *
 */

#ifndef RUNTIMECLASS_Microsoft_UI_Composition_Toolkit_CompositionGraphicsDevice_DEFINED
#define RUNTIMECLASS_Microsoft_UI_Composition_Toolkit_CompositionGraphicsDevice_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_Microsoft_UI_Composition_Toolkit_CompositionGraphicsDevice[] = L"Microsoft.UI.Composition.Toolkit.CompositionGraphicsDevice";
#endif


/*
 *
 * Class Microsoft.UI.Composition.Toolkit.CompositionImage
 *
 * Class implements the following interfaces:
 *    Microsoft.UI.Composition.Toolkit.ICompositionImage ** Default Interface **
 *
 * Class Threading Model:  Both Single and Multi Threaded Apartment
 *
 * Class Marshaling Behavior:  Agile - Class is agile
 *
 */

#ifndef RUNTIMECLASS_Microsoft_UI_Composition_Toolkit_CompositionImage_DEFINED
#define RUNTIMECLASS_Microsoft_UI_Composition_Toolkit_CompositionImage_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_Microsoft_UI_Composition_Toolkit_CompositionImage[] = L"Microsoft.UI.Composition.Toolkit.CompositionImage";
#endif


/*
 *
 * Class Microsoft.UI.Composition.Toolkit.CompositionImageFactory
 *
 * RuntimeClass contains static methods.
 *
 * Class implements the following interfaces:
 *    Microsoft.UI.Composition.Toolkit.ICompositionImageFactory ** Default Interface **
 *
 * Class Threading Model:  Both Single and Multi Threaded Apartment
 *
 * Class Marshaling Behavior:  Agile - Class is agile
 *
 */

#ifndef RUNTIMECLASS_Microsoft_UI_Composition_Toolkit_CompositionImageFactory_DEFINED
#define RUNTIMECLASS_Microsoft_UI_Composition_Toolkit_CompositionImageFactory_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_Microsoft_UI_Composition_Toolkit_CompositionImageFactory[] = L"Microsoft.UI.Composition.Toolkit.CompositionImageFactory";
#endif


/*
 *
 * Class Microsoft.UI.Composition.Toolkit.CompositionImageOptions
 *
 * RuntimeClass can be activated.
 *
 * Class implements the following interfaces:
 *    Microsoft.UI.Composition.Toolkit.ICompositionImageOptions ** Default Interface **
 *
 * Class Threading Model:  Both Single and Multi Threaded Apartment
 *
 * Class Marshaling Behavior:  Agile - Class is agile
 *
 */

#ifndef RUNTIMECLASS_Microsoft_UI_Composition_Toolkit_CompositionImageOptions_DEFINED
#define RUNTIMECLASS_Microsoft_UI_Composition_Toolkit_CompositionImageOptions_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_Microsoft_UI_Composition_Toolkit_CompositionImageOptions[] = L"Microsoft.UI.Composition.Toolkit.CompositionImageOptions";
#endif


#else // !defined(__cplusplus)
/* Forward Declarations */
#ifndef ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandler_FWD_DEFINED__
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandler_FWD_DEFINED__
typedef interface __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandler __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandler;

#endif // ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandler_FWD_DEFINED__

#ifndef ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandler_FWD_DEFINED__
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandler_FWD_DEFINED__
typedef interface __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandler __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandler;

#endif // ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandler_FWD_DEFINED__

#ifndef ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice_FWD_DEFINED__
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice_FWD_DEFINED__
typedef interface __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice;

#endif // ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice_FWD_DEFINED__

#ifndef ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics_FWD_DEFINED__
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics_FWD_DEFINED__
typedef interface __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics;

#endif // ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics_FWD_DEFINED__

#ifndef ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory_FWD_DEFINED__
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory_FWD_DEFINED__
typedef interface __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory;

#endif // ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory_FWD_DEFINED__

#ifndef ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics_FWD_DEFINED__
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics_FWD_DEFINED__
typedef interface __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics;

#endif // ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics_FWD_DEFINED__

#ifndef ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions_FWD_DEFINED__
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions_FWD_DEFINED__
typedef interface __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions;

#endif // ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions_FWD_DEFINED__

#ifndef ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage_FWD_DEFINED__
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage_FWD_DEFINED__
typedef interface __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage;

#endif // ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage_FWD_DEFINED__














typedef enum __x_Microsoft_CUI_CComposition_CToolkit_CCompositionImageLoadStatus __x_Microsoft_CUI_CComposition_CToolkit_CCompositionImageLoadStatus;


/*
 *
 * Struct Microsoft.UI.Composition.Toolkit.CompositionImageLoadStatus
 *
 */

/* [v1_enum, version] */
enum __x_Microsoft_CUI_CComposition_CToolkit_CCompositionImageLoadStatus
{
    CompositionImageLoadStatus_Success = 0,
    CompositionImageLoadStatus_FileAccessError = 1,
    CompositionImageLoadStatus_DecodeError = 2,
    CompositionImageLoadStatus_NotEnoughResources = 3,
    CompositionImageLoadStatus_Other = 4,
};


/*
 *
 * Delegate Microsoft.UI.Composition.Toolkit.CompositionGraphicsDeviceLostEventHandler
 *
 */
#if !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandler_INTERFACE_DEFINED__)
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandler_INTERFACE_DEFINED__
/* [object, version, uuid("0BE20CB6-FE18-3023-9680-F4AA2108BCEE")] */
typedef struct __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandlerVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandler * This,
    /* [in] */ __RPC__in REFIID riid,
    /* [annotation][iid_is][out] */
    _COM_Outptr_  void **ppvObject);

ULONG ( STDMETHODCALLTYPE *AddRef )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandler * This);

ULONG ( STDMETHODCALLTYPE *Release )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandler * This);
HRESULT ( STDMETHODCALLTYPE *Invoke )(
        __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandler * This,
        /* [in] */__x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice * sender
        );
    END_INTERFACE
    
} __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandlerVtbl;

interface __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandler
{
    CONST_VTBL struct __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandlerVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandler_QueryInterface(This,riid,ppvObject) \
( (This)->lpVtbl->QueryInterface(This,riid,ppvObject) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandler_AddRef(This) \
        ( (This)->lpVtbl->AddRef(This) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandler_Release(This) \
        ( (This)->lpVtbl->Release(This) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandler_Invoke(This,sender) \
    ( (This)->lpVtbl->Invoke(This,sender) )


#endif /* COBJMACROS */


EXTERN_C const IID IID___x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandler;
#endif /* !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandler_INTERFACE_DEFINED__) */


/*
 *
 * Delegate Microsoft.UI.Composition.Toolkit.CompositionImageLoadedEventHandler
 *
 */
#if !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandler_INTERFACE_DEFINED__)
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandler_INTERFACE_DEFINED__
/* [object, version, uuid("5B3AA813-CEBF-38B9-8F31-D23EB8C24F86")] */
typedef struct __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandlerVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandler * This,
    /* [in] */ __RPC__in REFIID riid,
    /* [annotation][iid_is][out] */
    _COM_Outptr_  void **ppvObject);

ULONG ( STDMETHODCALLTYPE *AddRef )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandler * This);

ULONG ( STDMETHODCALLTYPE *Release )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandler * This);
HRESULT ( STDMETHODCALLTYPE *Invoke )(
        __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandler * This,
        /* [in] */__x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage * sender,
        /* [in] */__x_Microsoft_CUI_CComposition_CToolkit_CCompositionImageLoadStatus status
        );
    END_INTERFACE
    
} __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandlerVtbl;

interface __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandler
{
    CONST_VTBL struct __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandlerVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandler_QueryInterface(This,riid,ppvObject) \
( (This)->lpVtbl->QueryInterface(This,riid,ppvObject) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandler_AddRef(This) \
        ( (This)->lpVtbl->AddRef(This) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandler_Release(This) \
        ( (This)->lpVtbl->Release(This) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandler_Invoke(This,sender,status) \
    ( (This)->lpVtbl->Invoke(This,sender,status) )


#endif /* COBJMACROS */


EXTERN_C const IID IID___x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandler;
#endif /* !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandler_INTERFACE_DEFINED__) */


/*
 *
 * Interface Microsoft.UI.Composition.Toolkit.ICompositionGraphicsDevice
 *
 * Interface is a part of the implementation of type Microsoft.UI.Composition.Toolkit.CompositionGraphicsDevice
 *
 *
 */
#if !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice_INTERFACE_DEFINED__)
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Microsoft_UI_Composition_Toolkit_ICompositionGraphicsDevice[] = L"Microsoft.UI.Composition.Toolkit.ICompositionGraphicsDevice";
/* [object, version, uuid("4A0E578E-68FA-3394-905A-D4D9D8AE30CA"), exclusiveto] */
typedef struct __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface)(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice * This,
    /* [in] */ __RPC__in REFIID riid,
    /* [annotation][iid_is][out] */
    _COM_Outptr_  void **ppvObject
    );

ULONG ( STDMETHODCALLTYPE *AddRef )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice * This
    );

ULONG ( STDMETHODCALLTYPE *Release )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice * This
    );

HRESULT ( STDMETHODCALLTYPE *GetIids )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice * This,
    /* [out] */ __RPC__out ULONG *iidCount,
    /* [size_is][size_is][out] */ __RPC__deref_out_ecount_full_opt(*iidCount) IID **iids
    );

HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice * This,
    /* [out] */ __RPC__deref_out_opt HSTRING *className
    );

HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice * This,
    /* [OUT ] */ __RPC__out TrustLevel *trustLevel
    );
/* [eventadd] */HRESULT ( STDMETHODCALLTYPE *add_DeviceLost )(
        __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice * This,
        /* [in] */__x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceLostEventHandler  * handler,
        /* [retval, out] */EventRegistrationToken * token
        );
    /* [eventremove] */HRESULT ( STDMETHODCALLTYPE *remove_DeviceLost )(
        __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice * This,
        /* [in] */EventRegistrationToken token
        );
    HRESULT ( STDMETHODCALLTYPE *CreateDrawingSurface )(
        __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice * This,
        /* [in] */__x_Windows_CFoundation_CSize sizePixels,
        /* [in] */__x_Windows_CGraphics_CDirectX_CDirectXPixelFormat pixelFormat,
        /* [in] */__x_Windows_CGraphics_CDirectX_CDirectXAlphaMode alphaMode,
        /* [retval, out] */__x_Windows_CUI_CComposition_CICompositionSurface * * surface
        );
    HRESULT ( STDMETHODCALLTYPE *AcquireDrawingLock )(
        __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice * This
        );
    HRESULT ( STDMETHODCALLTYPE *ReleaseDrawingLock )(
        __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice * This
        );
    END_INTERFACE
    
} __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceVtbl;

interface __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice
{
    CONST_VTBL struct __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice_QueryInterface(This,riid,ppvObject) \
( (This)->lpVtbl->QueryInterface(This,riid,ppvObject) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice_AddRef(This) \
        ( (This)->lpVtbl->AddRef(This) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice_Release(This) \
        ( (This)->lpVtbl->Release(This) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice_GetIids(This,iidCount,iids) \
        ( (This)->lpVtbl->GetIids(This,iidCount,iids) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice_GetRuntimeClassName(This,className) \
        ( (This)->lpVtbl->GetRuntimeClassName(This,className) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice_GetTrustLevel(This,trustLevel) \
        ( (This)->lpVtbl->GetTrustLevel(This,trustLevel) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice_add_DeviceLost(This,handler,token) \
    ( (This)->lpVtbl->add_DeviceLost(This,handler,token) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice_remove_DeviceLost(This,token) \
    ( (This)->lpVtbl->remove_DeviceLost(This,token) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice_CreateDrawingSurface(This,sizePixels,pixelFormat,alphaMode,surface) \
    ( (This)->lpVtbl->CreateDrawingSurface(This,sizePixels,pixelFormat,alphaMode,surface) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice_AcquireDrawingLock(This) \
    ( (This)->lpVtbl->AcquireDrawingLock(This) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice_ReleaseDrawingLock(This) \
    ( (This)->lpVtbl->ReleaseDrawingLock(This) )


#endif /* COBJMACROS */


EXTERN_C const IID IID___x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice;
#endif /* !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice_INTERFACE_DEFINED__) */


/*
 *
 * Interface Microsoft.UI.Composition.Toolkit.ICompositionGraphicsDeviceStatics
 *
 * Interface is a part of the implementation of type Microsoft.UI.Composition.Toolkit.CompositionGraphicsDevice
 *
 *
 */
#if !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics_INTERFACE_DEFINED__)
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Microsoft_UI_Composition_Toolkit_ICompositionGraphicsDeviceStatics[] = L"Microsoft.UI.Composition.Toolkit.ICompositionGraphicsDeviceStatics";
/* [object, version, uuid("BC22813B-C736-3176-9D45-75F86BEAE8B6"), exclusiveto] */
typedef struct __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStaticsVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface)(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics * This,
    /* [in] */ __RPC__in REFIID riid,
    /* [annotation][iid_is][out] */
    _COM_Outptr_  void **ppvObject
    );

ULONG ( STDMETHODCALLTYPE *AddRef )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics * This
    );

ULONG ( STDMETHODCALLTYPE *Release )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics * This
    );

HRESULT ( STDMETHODCALLTYPE *GetIids )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics * This,
    /* [out] */ __RPC__out ULONG *iidCount,
    /* [size_is][size_is][out] */ __RPC__deref_out_ecount_full_opt(*iidCount) IID **iids
    );

HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics * This,
    /* [out] */ __RPC__deref_out_opt HSTRING *className
    );

HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics * This,
    /* [OUT ] */ __RPC__out TrustLevel *trustLevel
    );
HRESULT ( STDMETHODCALLTYPE *CreateCompositionGraphicsDevice )(
        __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics * This,
        /* [in] */__x_Windows_CUI_CComposition_CICompositor * compositor,
        /* [retval, out] */__x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDevice * * device
        );
    END_INTERFACE
    
} __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStaticsVtbl;

interface __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics
{
    CONST_VTBL struct __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStaticsVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics_QueryInterface(This,riid,ppvObject) \
( (This)->lpVtbl->QueryInterface(This,riid,ppvObject) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics_AddRef(This) \
        ( (This)->lpVtbl->AddRef(This) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics_Release(This) \
        ( (This)->lpVtbl->Release(This) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics_GetIids(This,iidCount,iids) \
        ( (This)->lpVtbl->GetIids(This,iidCount,iids) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics_GetRuntimeClassName(This,className) \
        ( (This)->lpVtbl->GetRuntimeClassName(This,className) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics_GetTrustLevel(This,trustLevel) \
        ( (This)->lpVtbl->GetTrustLevel(This,trustLevel) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics_CreateCompositionGraphicsDevice(This,compositor,device) \
    ( (This)->lpVtbl->CreateCompositionGraphicsDevice(This,compositor,device) )


#endif /* COBJMACROS */


EXTERN_C const IID IID___x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics;
#endif /* !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionGraphicsDeviceStatics_INTERFACE_DEFINED__) */


/*
 *
 * Interface Microsoft.UI.Composition.Toolkit.ICompositionImageFactory
 *
 * Interface is a part of the implementation of type Microsoft.UI.Composition.Toolkit.CompositionImageFactory
 *
 *
 */
#if !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory_INTERFACE_DEFINED__)
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Microsoft_UI_Composition_Toolkit_ICompositionImageFactory[] = L"Microsoft.UI.Composition.Toolkit.ICompositionImageFactory";
/* [object, version, uuid("25372FEC-3FA1-3C03-980F-FC48BA44680F"), exclusiveto] */
typedef struct __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface)(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory * This,
    /* [in] */ __RPC__in REFIID riid,
    /* [annotation][iid_is][out] */
    _COM_Outptr_  void **ppvObject
    );

ULONG ( STDMETHODCALLTYPE *AddRef )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory * This
    );

ULONG ( STDMETHODCALLTYPE *Release )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory * This
    );

HRESULT ( STDMETHODCALLTYPE *GetIids )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory * This,
    /* [out] */ __RPC__out ULONG *iidCount,
    /* [size_is][size_is][out] */ __RPC__deref_out_ecount_full_opt(*iidCount) IID **iids
    );

HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory * This,
    /* [out] */ __RPC__deref_out_opt HSTRING *className
    );

HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory * This,
    /* [OUT ] */ __RPC__out TrustLevel *trustLevel
    );
/* [overload] */HRESULT ( STDMETHODCALLTYPE *CreateImageFromUri2 )(
        __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory * This,
        /* [in] */__x_Windows_CFoundation_CIUriRuntimeClass * uri,
        /* [retval, out] */__x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage * * image
        );
    /* [overload] */HRESULT ( STDMETHODCALLTYPE *CreateImageFromUri1 )(
        __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory * This,
        /* [in] */__x_Windows_CFoundation_CIUriRuntimeClass * uri,
        /* [in] */__x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions * options,
        /* [retval, out] */__x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage * * image
        );
    /* [overload] */HRESULT ( STDMETHODCALLTYPE *CreateImageFromFile2 )(
        __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory * This,
        /* [in] */__x_Windows_CStorage_CIStorageFile * file,
        /* [retval, out] */__x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage * * image
        );
    /* [overload] */HRESULT ( STDMETHODCALLTYPE *CreateImageFromFile1 )(
        __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory * This,
        /* [in] */__x_Windows_CStorage_CIStorageFile * file,
        /* [in] */__x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions * options,
        /* [retval, out] */__x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage * * image
        );
    HRESULT ( STDMETHODCALLTYPE *CreateImageFromPixels )(
        __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory * This,
        /* [in] */UINT32 pixelsSize,
        /* [size_is(pixelsSize), in] */BYTE * pixels,
        /* [in] */INT32 pixelWidth,
        /* [in] */INT32 pixelHeight,
        /* [retval, out] */__x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage * * image
        );
    END_INTERFACE
    
} __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryVtbl;

interface __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory
{
    CONST_VTBL struct __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory_QueryInterface(This,riid,ppvObject) \
( (This)->lpVtbl->QueryInterface(This,riid,ppvObject) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory_AddRef(This) \
        ( (This)->lpVtbl->AddRef(This) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory_Release(This) \
        ( (This)->lpVtbl->Release(This) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory_GetIids(This,iidCount,iids) \
        ( (This)->lpVtbl->GetIids(This,iidCount,iids) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory_GetRuntimeClassName(This,className) \
        ( (This)->lpVtbl->GetRuntimeClassName(This,className) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory_GetTrustLevel(This,trustLevel) \
        ( (This)->lpVtbl->GetTrustLevel(This,trustLevel) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory_CreateImageFromUri2(This,uri,image) \
    ( (This)->lpVtbl->CreateImageFromUri2(This,uri,image) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory_CreateImageFromUri1(This,uri,options,image) \
    ( (This)->lpVtbl->CreateImageFromUri1(This,uri,options,image) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory_CreateImageFromFile2(This,file,image) \
    ( (This)->lpVtbl->CreateImageFromFile2(This,file,image) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory_CreateImageFromFile1(This,file,options,image) \
    ( (This)->lpVtbl->CreateImageFromFile1(This,file,options,image) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory_CreateImageFromPixels(This,pixelsSize,pixels,pixelWidth,pixelHeight,image) \
    ( (This)->lpVtbl->CreateImageFromPixels(This,pixelsSize,pixels,pixelWidth,pixelHeight,image) )


#endif /* COBJMACROS */


EXTERN_C const IID IID___x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory;
#endif /* !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory_INTERFACE_DEFINED__) */


/*
 *
 * Interface Microsoft.UI.Composition.Toolkit.ICompositionImageFactoryStatics
 *
 * Interface is a part of the implementation of type Microsoft.UI.Composition.Toolkit.CompositionImageFactory
 *
 *
 */
#if !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics_INTERFACE_DEFINED__)
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Microsoft_UI_Composition_Toolkit_ICompositionImageFactoryStatics[] = L"Microsoft.UI.Composition.Toolkit.ICompositionImageFactoryStatics";
/* [object, version, uuid("EB167AED-6BF0-39AE-90B1-7C05FF0F308F"), exclusiveto] */
typedef struct __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStaticsVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface)(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics * This,
    /* [in] */ __RPC__in REFIID riid,
    /* [annotation][iid_is][out] */
    _COM_Outptr_  void **ppvObject
    );

ULONG ( STDMETHODCALLTYPE *AddRef )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics * This
    );

ULONG ( STDMETHODCALLTYPE *Release )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics * This
    );

HRESULT ( STDMETHODCALLTYPE *GetIids )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics * This,
    /* [out] */ __RPC__out ULONG *iidCount,
    /* [size_is][size_is][out] */ __RPC__deref_out_ecount_full_opt(*iidCount) IID **iids
    );

HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics * This,
    /* [out] */ __RPC__deref_out_opt HSTRING *className
    );

HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics * This,
    /* [OUT ] */ __RPC__out TrustLevel *trustLevel
    );
HRESULT ( STDMETHODCALLTYPE *CreateCompositionImageFactory )(
        __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics * This,
        /* [in] */__x_Windows_CUI_CComposition_CICompositor * compositor,
        /* [retval, out] */__x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactory * * factory
        );
    END_INTERFACE
    
} __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStaticsVtbl;

interface __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics
{
    CONST_VTBL struct __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStaticsVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics_QueryInterface(This,riid,ppvObject) \
( (This)->lpVtbl->QueryInterface(This,riid,ppvObject) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics_AddRef(This) \
        ( (This)->lpVtbl->AddRef(This) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics_Release(This) \
        ( (This)->lpVtbl->Release(This) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics_GetIids(This,iidCount,iids) \
        ( (This)->lpVtbl->GetIids(This,iidCount,iids) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics_GetRuntimeClassName(This,className) \
        ( (This)->lpVtbl->GetRuntimeClassName(This,className) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics_GetTrustLevel(This,trustLevel) \
        ( (This)->lpVtbl->GetTrustLevel(This,trustLevel) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics_CreateCompositionImageFactory(This,compositor,factory) \
    ( (This)->lpVtbl->CreateCompositionImageFactory(This,compositor,factory) )


#endif /* COBJMACROS */


EXTERN_C const IID IID___x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics;
#endif /* !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageFactoryStatics_INTERFACE_DEFINED__) */


/*
 *
 * Interface Microsoft.UI.Composition.Toolkit.ICompositionImageOptions
 *
 * Interface is a part of the implementation of type Microsoft.UI.Composition.Toolkit.CompositionImageOptions
 *
 *
 */
#if !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions_INTERFACE_DEFINED__)
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Microsoft_UI_Composition_Toolkit_ICompositionImageOptions[] = L"Microsoft.UI.Composition.Toolkit.ICompositionImageOptions";
/* [object, version, uuid("8554C61C-F17B-3602-868B-C17C9F7BC449"), exclusiveto] */
typedef struct __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptionsVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface)(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions * This,
    /* [in] */ __RPC__in REFIID riid,
    /* [annotation][iid_is][out] */
    _COM_Outptr_  void **ppvObject
    );

ULONG ( STDMETHODCALLTYPE *AddRef )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions * This
    );

ULONG ( STDMETHODCALLTYPE *Release )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions * This
    );

HRESULT ( STDMETHODCALLTYPE *GetIids )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions * This,
    /* [out] */ __RPC__out ULONG *iidCount,
    /* [size_is][size_is][out] */ __RPC__deref_out_ecount_full_opt(*iidCount) IID **iids
    );

HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions * This,
    /* [out] */ __RPC__deref_out_opt HSTRING *className
    );

HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions * This,
    /* [OUT ] */ __RPC__out TrustLevel *trustLevel
    );
/* [propget] */HRESULT ( STDMETHODCALLTYPE *get_DecodeWidth )(
        __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions * This,
        /* [retval, out] */INT32 * value
        );
    /* [propput] */HRESULT ( STDMETHODCALLTYPE *put_DecodeWidth )(
        __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions * This,
        /* [in] */INT32 value
        );
    /* [propget] */HRESULT ( STDMETHODCALLTYPE *get_DecodeHeight )(
        __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions * This,
        /* [retval, out] */INT32 * value
        );
    /* [propput] */HRESULT ( STDMETHODCALLTYPE *put_DecodeHeight )(
        __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions * This,
        /* [in] */INT32 value
        );
    END_INTERFACE
    
} __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptionsVtbl;

interface __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions
{
    CONST_VTBL struct __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptionsVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions_QueryInterface(This,riid,ppvObject) \
( (This)->lpVtbl->QueryInterface(This,riid,ppvObject) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions_AddRef(This) \
        ( (This)->lpVtbl->AddRef(This) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions_Release(This) \
        ( (This)->lpVtbl->Release(This) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions_GetIids(This,iidCount,iids) \
        ( (This)->lpVtbl->GetIids(This,iidCount,iids) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions_GetRuntimeClassName(This,className) \
        ( (This)->lpVtbl->GetRuntimeClassName(This,className) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions_GetTrustLevel(This,trustLevel) \
        ( (This)->lpVtbl->GetTrustLevel(This,trustLevel) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions_get_DecodeWidth(This,value) \
    ( (This)->lpVtbl->get_DecodeWidth(This,value) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions_put_DecodeWidth(This,value) \
    ( (This)->lpVtbl->put_DecodeWidth(This,value) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions_get_DecodeHeight(This,value) \
    ( (This)->lpVtbl->get_DecodeHeight(This,value) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions_put_DecodeHeight(This,value) \
    ( (This)->lpVtbl->put_DecodeHeight(This,value) )


#endif /* COBJMACROS */


EXTERN_C const IID IID___x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions;
#endif /* !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageOptions_INTERFACE_DEFINED__) */


/*
 *
 * Interface Microsoft.UI.Composition.Toolkit.ICompositionImage
 *
 * Interface is a part of the implementation of type Microsoft.UI.Composition.Toolkit.CompositionImage
 *
 *
 */
#if !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage_INTERFACE_DEFINED__)
#define ____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Microsoft_UI_Composition_Toolkit_ICompositionImage[] = L"Microsoft.UI.Composition.Toolkit.ICompositionImage";
/* [object, version, uuid("D5E4B2E9-82F7-3A11-8D7A-E58B892E054C"), exclusiveto] */
typedef struct __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface)(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage * This,
    /* [in] */ __RPC__in REFIID riid,
    /* [annotation][iid_is][out] */
    _COM_Outptr_  void **ppvObject
    );

ULONG ( STDMETHODCALLTYPE *AddRef )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage * This
    );

ULONG ( STDMETHODCALLTYPE *Release )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage * This
    );

HRESULT ( STDMETHODCALLTYPE *GetIids )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage * This,
    /* [out] */ __RPC__out ULONG *iidCount,
    /* [size_is][size_is][out] */ __RPC__deref_out_ecount_full_opt(*iidCount) IID **iids
    );

HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage * This,
    /* [out] */ __RPC__deref_out_opt HSTRING *className
    );

HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )(
    __RPC__in __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage * This,
    /* [OUT ] */ __RPC__out TrustLevel *trustLevel
    );
/* [eventadd] */HRESULT ( STDMETHODCALLTYPE *add_ImageLoaded )(
        __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage * This,
        /* [in] */__x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageLoadedEventHandler  * handler,
        /* [retval, out] */EventRegistrationToken * token
        );
    /* [eventremove] */HRESULT ( STDMETHODCALLTYPE *remove_ImageLoaded )(
        __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage * This,
        /* [in] */EventRegistrationToken token
        );
    /* [propget] */HRESULT ( STDMETHODCALLTYPE *get_Size )(
        __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage * This,
        /* [retval, out] */__x_Windows_CFoundation_CSize * value
        );
    /* [propget] */HRESULT ( STDMETHODCALLTYPE *get_Surface )(
        __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage * This,
        /* [retval, out] */__x_Windows_CUI_CComposition_CICompositionSurface * * value
        );
    END_INTERFACE
    
} __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageVtbl;

interface __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage
{
    CONST_VTBL struct __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImageVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage_QueryInterface(This,riid,ppvObject) \
( (This)->lpVtbl->QueryInterface(This,riid,ppvObject) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage_AddRef(This) \
        ( (This)->lpVtbl->AddRef(This) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage_Release(This) \
        ( (This)->lpVtbl->Release(This) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage_GetIids(This,iidCount,iids) \
        ( (This)->lpVtbl->GetIids(This,iidCount,iids) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage_GetRuntimeClassName(This,className) \
        ( (This)->lpVtbl->GetRuntimeClassName(This,className) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage_GetTrustLevel(This,trustLevel) \
        ( (This)->lpVtbl->GetTrustLevel(This,trustLevel) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage_add_ImageLoaded(This,handler,token) \
    ( (This)->lpVtbl->add_ImageLoaded(This,handler,token) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage_remove_ImageLoaded(This,token) \
    ( (This)->lpVtbl->remove_ImageLoaded(This,token) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage_get_Size(This,value) \
    ( (This)->lpVtbl->get_Size(This,value) )

#define __x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage_get_Surface(This,value) \
    ( (This)->lpVtbl->get_Surface(This,value) )


#endif /* COBJMACROS */


EXTERN_C const IID IID___x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage;
#endif /* !defined(____x_Microsoft_CUI_CComposition_CToolkit_CICompositionImage_INTERFACE_DEFINED__) */


/*
 *
 * Class Microsoft.UI.Composition.Toolkit.CompositionGraphicsDevice
 *
 * RuntimeClass contains static methods.
 *
 * Class implements the following interfaces:
 *    Microsoft.UI.Composition.Toolkit.ICompositionGraphicsDevice ** Default Interface **
 *    Windows.Foundation.IClosable
 *
 * Class Threading Model:  Both Single and Multi Threaded Apartment
 *
 * Class Marshaling Behavior:  Agile - Class is agile
 *
 */

#ifndef RUNTIMECLASS_Microsoft_UI_Composition_Toolkit_CompositionGraphicsDevice_DEFINED
#define RUNTIMECLASS_Microsoft_UI_Composition_Toolkit_CompositionGraphicsDevice_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_Microsoft_UI_Composition_Toolkit_CompositionGraphicsDevice[] = L"Microsoft.UI.Composition.Toolkit.CompositionGraphicsDevice";
#endif


/*
 *
 * Class Microsoft.UI.Composition.Toolkit.CompositionImage
 *
 * Class implements the following interfaces:
 *    Microsoft.UI.Composition.Toolkit.ICompositionImage ** Default Interface **
 *
 * Class Threading Model:  Both Single and Multi Threaded Apartment
 *
 * Class Marshaling Behavior:  Agile - Class is agile
 *
 */

#ifndef RUNTIMECLASS_Microsoft_UI_Composition_Toolkit_CompositionImage_DEFINED
#define RUNTIMECLASS_Microsoft_UI_Composition_Toolkit_CompositionImage_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_Microsoft_UI_Composition_Toolkit_CompositionImage[] = L"Microsoft.UI.Composition.Toolkit.CompositionImage";
#endif


/*
 *
 * Class Microsoft.UI.Composition.Toolkit.CompositionImageFactory
 *
 * RuntimeClass contains static methods.
 *
 * Class implements the following interfaces:
 *    Microsoft.UI.Composition.Toolkit.ICompositionImageFactory ** Default Interface **
 *
 * Class Threading Model:  Both Single and Multi Threaded Apartment
 *
 * Class Marshaling Behavior:  Agile - Class is agile
 *
 */

#ifndef RUNTIMECLASS_Microsoft_UI_Composition_Toolkit_CompositionImageFactory_DEFINED
#define RUNTIMECLASS_Microsoft_UI_Composition_Toolkit_CompositionImageFactory_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_Microsoft_UI_Composition_Toolkit_CompositionImageFactory[] = L"Microsoft.UI.Composition.Toolkit.CompositionImageFactory";
#endif


/*
 *
 * Class Microsoft.UI.Composition.Toolkit.CompositionImageOptions
 *
 * RuntimeClass can be activated.
 *
 * Class implements the following interfaces:
 *    Microsoft.UI.Composition.Toolkit.ICompositionImageOptions ** Default Interface **
 *
 * Class Threading Model:  Both Single and Multi Threaded Apartment
 *
 * Class Marshaling Behavior:  Agile - Class is agile
 *
 */

#ifndef RUNTIMECLASS_Microsoft_UI_Composition_Toolkit_CompositionImageOptions_DEFINED
#define RUNTIMECLASS_Microsoft_UI_Composition_Toolkit_CompositionImageOptions_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_Microsoft_UI_Composition_Toolkit_CompositionImageOptions[] = L"Microsoft.UI.Composition.Toolkit.CompositionImageOptions";
#endif


#endif // defined(__cplusplus)
#pragma pop_macro("MIDL_CONST_ID")
#endif // __Microsoft2EUI2EComposition2EToolkit_h_p_h__

#endif // __Microsoft2EUI2EComposition2EToolkit_h_h__
