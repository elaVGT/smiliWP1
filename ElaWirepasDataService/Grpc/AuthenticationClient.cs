using ElaAuthentication;
using ElaCommon;
using elaMicroservicesGrpc.Constant;
using ElaWirepas;
using Grpc.Core;
using System;

namespace ElaWirepasDataService.Grpc
{
    public class AuthenticationClient
    {

        private const String REAL_LOGIN = "elainnovation";
        private const String REAL_CERTIFICATE = "{\"data\":\"4e2aaabf6d91030218dc624821d38a6b2bbbfd329e8543af1ce1882589aa3e7ad640bc03e91033ddce426e7deba6548536de0f647760635090eaed9b1264701f4cf498c14007daec747f59ff6b5b5ee88d7563b49455fa51d45e9c89ee8ac03ad27a83e307b5301aa508bbad78ab512759c3ad95ee24187e097c797c638d105d\",\"sign\":\"bdee89baafa0a6f2db0206f16e0d14a657d8d6e10221571f9b998ca4c47d3b0ff5b8de770d046d52b1fa271908c61c0df6e4db83c3c217a233a8e74c5524a85f9affdd6d522588a167a0e7936e857f77c85c1da61f98a0461c8e6c80b836142a1f88115f74fdfb4de80b497bbe40a996b2172462482466493c636abe4099e500\"}";

        private ElaAuthenticationPublicService.ElaAuthenticationPublicServiceClient client = null;

        private String m_internalSessionId = String.Empty;
        /* constructor */
        public AuthenticationClient()
        {
            client = new ElaAuthenticationPublicService.ElaAuthenticationPublicServiceClient(new Channel("192.168.0.66", ElaGrpcConstants.PORT_AUTHENTICATION_REMOTE_API, ChannelCredentials.Insecure));
        }
        //public ElaManagerUserResponse LoginWirepasMicroservice(ElaWirepasAuthenticationRequest request)
        //{
        //    try { 

        //        ElaInputBaseRequest internalRequest = new ElaInputBaseRequest() { ClientId = String.Empty, RequestId = String.Empty, SessionId = String.Empty };
        //        ElaAuthenticationRequest authReq = new ElaAuthenticationRequest() { Login = REAL_LOGIN, Certificate = REAL_CERTIFICATE, Request = internalRequest };
        //        //
        //        ElaAuthenticationResponse internalresponse = this.client.Login(authReq);
        //        m_internalSessionId = internalresponse.SessionId;

        //        ElaInputBaseRequest checkRequest = new ElaInputBaseRequest() { ClientId = String.Empty, RequestId = "1" , SessionId = m_internalSessionId };

        //        var response = this.client.CheckAuthorization(new ElaManagerUserRequest { User = new ElaAuthenticationUser { Username = request.Username, Password = request.Password }, Request = checkRequest });
        //        //
        //        ElaInputBaseRequest logoutRequest = new ElaInputBaseRequest() { ClientId = String.Empty, RequestId = "2", SessionId = m_internalSessionId };
        //        this.client.Logout(logoutRequest);
        //        //
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return new ElaManagerUserResponse();
        //    }
        //}

    }
}
