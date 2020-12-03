using ElaAuthentication;
using ElaCommon;
using elaMicroservicesGrpc;
using ElaWirepas;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

/**
 * \namespace ElaWirepasDataService.Grpc.Services
 * \brief implementation on the server side of the microservices functions
 */
namespace ElaWirepasDataService.Grpc.Services
{
    //public class AuthenticationService : ElaWirepasConfigurationService.ElaWirepasConfigurationServiceBase
    //{
    //    /**
    //     * \fn CheckAuthentication
    //     * \brief CheckAuthentication function to check users authentication through Ela Authetiction service
    //     * \param [in] ElaWirepasAuthenticationRequest : Authentication request
    //     * 
    //     * \return ElaWirepasAuthenticationResponse with authentication response
    //     */
    //    //public override Task<ElaWirepasAuthenticationResponse> CheckAuthentication(ElaWirepasAuthenticationRequest request, ServerCallContext context)
    //    //{
    //    //    ElaWirepasAuthenticationResponse AuthResponse = new ElaWirepasAuthenticationResponse() { Athorized = false, Error = new ElaError() };

    //    //    try
    //    //    {
    //    //        ElaManagerUserResponse response = new AuthenticationClient().LoginWirepasMicroservice(request);

    //    //        AuthResponse.Athorized = response.AllowedUser;
    //    //        AuthResponse.Error = response.Error;
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        Console.WriteLine(ex.Message);
    //    //    }

    //    //    return Task.FromResult(AuthResponse);
    //    //}
    //}
}
