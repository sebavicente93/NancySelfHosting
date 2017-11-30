namespace NancySelfHosting.Modules
{
    using Nancy;
    using Nancy.ModelBinding;
    using Nancy.Validation;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using NancySelfHosting.DB;
    using NancySelfHosting.Entities;

    public class ClientModule : NancyModule
    {
        public ClientModule() : base("/client")
        {
 
            Get("/list",  (_) =>
            {
                List<ClientEntity> client = GetPeople();
                return Response.AsJson<List<ClientEntity>>(client);
            });

            Post("/", (_) =>
            {
                var clientEntity = this.Bind<ClientEntity>();
                
                var validationResult = this.Validate(clientEntity);
                if (!validationResult.IsValid)
                {
                    return GetValidationResponse(validationResult);
                }
                return PostPeople(clientEntity);
            });

            Get("/{id:int}", (parameters) =>
            {
                ClientEntity client = GetClient(parameters.id);
                if (client != null)
                {
                    return Response.AsJson<ClientEntity>(client);
                }else
                {
                    return HttpStatusCode.NotFound;
                }
                
            });


            Delete("/{id:int}", (parameters) =>
            {
               return DeleteClient(parameters.id);
            });

            Patch("/{id:int}", (parameters) =>
            {
                ClientEntity client = SingletonClientsInMemory.Instance.Get(parameters.id);
                if (client == null)
                {
                    return HttpStatusCode.NotFound;
                } 

                ClientEntity clientPatch = this.Bind<ClientEntity>();

                // _id no puede ser cambiado
                clientPatch._id = null;

                // Itera por cada propiedad definida en ClientEntity, de está forma es posible extender la entidad sin
                // modificar este método
                System.Reflection.PropertyInfo[] properties = typeof(ClientEntity).GetProperties();
                foreach (PropertyInfo property in properties)
                {                  
                    if (property.GetValue(clientPatch, null) == null)
                    {
                        property.SetValue(clientPatch, property.GetValue(client, null));
                    }
                }

                // Chequea si los campos son correctos y realiza la actualizacion en la DB
                var validationResult = this.Validate(clientPatch);
                if (!validationResult.IsValid)
                {
                    return GetValidationResponse(validationResult);
                } else
                {
                    SingletonClientsInMemory.Instance.Replace(clientPatch);
                }

                return Response.AsJson(client);
            });
        }

        private Response GetValidationResponse(ModelValidationResult validationResult)
        {
            
                var errors = String.Empty;
                foreach (var error in validationResult.Errors)
                {
                    errors += error.Value[0].ErrorMessage + Environment.NewLine;
                }

                var response = (Response)errors;
                response.StatusCode = HttpStatusCode.BadRequest;

                return response;
            
        }

        private List<ClientEntity> GetPeople()
        {
            return SingletonClientsInMemory.Instance.GetAll();
        }

        private HttpStatusCode PostPeople(ClientEntity personEntity)
        {
            try
            {
                SingletonClientsInMemory.Instance.Add(personEntity);
                return HttpStatusCode.Created;
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }
        }

        private ClientEntity GetClient(string id)
        {
            return SingletonClientsInMemory.Instance.Get(id);
        }

        private HttpStatusCode DeleteClient(string id)
        {
            var aux= SingletonClientsInMemory.Instance.Delete(id);
            if(aux != null)
            {
                return HttpStatusCode.OK;
            }else
            {
                return HttpStatusCode.NotFound;
            }   
        }
    }
}