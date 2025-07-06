using Relations.GRPC;
namespace Post.API.GrpcServices;

public class GreeterService
{
    private readonly Greeter.GreeterClient _greeterService;


    public GreeterService(Greeter.GreeterClient greeterService)
    {
        _greeterService = greeterService ?? throw new ArgumentNullException(nameof(greeterService));
    }
    public string SayHello()
    {
        HelloRequest x = new HelloRequest() {Name="lalal" };

        var response =_greeterService.SayHello(x);
        return response.Message;
    }


}
