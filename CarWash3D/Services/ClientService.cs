using CarWash3D.Data.Repositories;
using CarWash3D.Models;

namespace CarWash3D.Services
{
    public class ClientService
    {
        private readonly ClientRepository _clientRepository;

        public ClientService(ClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public bool AuthenticateClient(string phone, string carNumber)
        {
            var client = _clientRepository.GetClientByPhoneAndCarNumber(phone, carNumber);
            return client != null;
        }

        public bool RegisterClient(Client client)
        {
            return _clientRepository.AddClient(client);
        }

        public bool VerifyCode(string phone, string code)
        {
            // Заглушка: статичный код "123456"
            return code == "123456";
        }
        public Client GetClientByPhoneNumber(string phone)
        {
            var client = _clientRepository.GetClientByPhone(phone);
            return client;
        }
    }
}