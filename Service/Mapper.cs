using CharityManager.DTO;
using CharityManager.Service.EntityFramework;
using System;
using System.Linq;

namespace CharityManager.Service
{
    public static class Mapper
    {
        public static TResult Map<T, TResult>(T source,params string[] ignore)
        {
            if (source == null)
                return default;

            var destination = Activator.CreateInstance<TResult>();
            var destProps = destination.GetType().GetProperties();

            var srcProps = source.GetType().GetProperties();

            foreach (var prop in destProps)
            {
                if (ignore!= null && ignore.Contains(prop.Name))
                    continue;

                var sp = srcProps.FirstOrDefault(src => src.Name == prop.Name);
                if (sp == null) continue;

                var value = sp.GetValue(source);
                prop.SetValue(destination, value);
            }

            return destination;
        }

        public static Func<PersonDTO, Person> PersonMapper = new Func<PersonDTO, Person>(entity => Map<PersonDTO, Person>(entity));
        public static Func<Person, PersonDTO> PersonDTOMapper = new Func<Person, PersonDTO>(entity => Map<Person, PersonDTO>(entity));

        public static Func<UserDTO, User> UserMapper = new Func<UserDTO, User>(dto => Map<UserDTO, User>(dto));
        public static Func<User, UserDTO> UserDTOMapper = new Func<User, UserDTO>(entity => Map<User, UserDTO>(entity));

        public static Func<PatronDTO, Patron> PatronMapper = new Func<PatronDTO, Patron>(dto => Map<PatronDTO, Patron>(dto));
        public static Func<Patron, PatronDTO> PatronDTOMapper = new Func<Patron, PatronDTO>(entity => Map<Patron, PatronDTO>(entity,"Person"));

        public static Func<AddressDTO, Address> AddressMapper = new Func<AddressDTO, Address>(dto => Map<AddressDTO, Address>(dto));
        public static Func<Address, AddressDTO> AddressDTOMapper = new Func<Address, AddressDTO>(entity => Map<Address, AddressDTO>(entity));

        public static Func<FamilyDTO, Family> FamilyMapper = new Func<FamilyDTO, Family>(dto => Map<FamilyDTO, Family>(dto));
        public static Func<Family, FamilyDTO> FamilyDTOMapper = new Func<Family, FamilyDTO>(entity => Map<Family, FamilyDTO>(entity));

        public static Func<JobDTO, Job> JobMapper = new Func<JobDTO, Job>(dto => Map<JobDTO, Job>(dto));
        public static Func<Job, JobDTO> JobDTOMapper = new Func<Job, JobDTO>(entity => Map<Job, JobDTO>(entity));

        public static Func<AssetDTO, Asset> AssetMapper = new Func<AssetDTO, Asset>(dto => Map<AssetDTO, Asset>(dto));
        public static Func<Asset, AssetDTO> AssetDTOMapper = new Func<Asset, AssetDTO>(entity => Map<Asset, AssetDTO>(entity));

        public static Func<EntityDTO, Entity> EntityMapper = new Func<EntityDTO, Entity>(dto => Map<EntityDTO, Entity>(dto));
        public static Func<Entity, EntityDTO> EntityDTOMapper = new Func<Entity, EntityDTO>(entity => Map<Entity, EntityDTO>(entity));

        public static Func<PictureDTO, Picture> PictureMapper = new Func<PictureDTO, Picture>(entity => Map<PictureDTO, Picture>(entity));
        public static Func<Picture, PictureDTO> PictureDTOMapper = new Func<Picture, PictureDTO>(entity => Map<Picture, PictureDTO>(entity));

        public static Func<DocumentDTO, Document> DocumentMapper = new Func<DocumentDTO, Document>(entity => Map<DocumentDTO, Document>(entity));
        public static Func<Document, DocumentDTO> DocumentDTOMapper = new Func<Document, DocumentDTO>(entity => Map<Document, DocumentDTO>(entity));

    }
}
