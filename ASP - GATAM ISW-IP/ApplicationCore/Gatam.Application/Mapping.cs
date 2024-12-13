using Gatam.Application.CQRS;
using AutoMapper;
using Gatam.Domain;
using Gatam.Application.CQRS.Questions;
using Gatam.Application.CQRS.DTOS.QuestionsDTO;
using Gatam.Application.CQRS.DTOS.ModulesDTO;
using Gatam.Application.CQRS.DTOS.UsersDTO;
using Gatam.Application.CQRS.DTOS.RolesDTO;
namespace Gatam.Application
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ApplicationUser, UserDTO>()
            .ForMember(dest => dest.RolesIds, opt => opt.MapFrom(src => src.UserRoles));
            CreateMap<UserDTO, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore());

            CreateMap<ApplicationUser, CreateUserDTO>()
            .ForMember(dest => dest.RolesIds, opt => opt.MapFrom(src => src.UserRoles))
                                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
            CreateMap<CreateUserDTO, ApplicationUser>()
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore());




            CreateMap<UserRole, UserRoleDTO>();
            CreateMap<UserRoleDTO, UserRole>()
                                                .ForMember(dest => dest.User, opt => opt.Ignore());





            CreateMap<ApplicationModule, ModuleDTO>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                 .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                 .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                 .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));

            CreateMap<ModuleDTO, ApplicationModule>()
                .ForMember(dest => dest.Questions, opt => opt.Ignore());

            CreateMap<Question, QuestionDTO>()
                 .ForMember(dest => dest.UserQuestion, opt => opt.MapFrom(src => src.UserQuestions.FirstOrDefault()))
                 .ForMember(dest => dest.QuestionAnswers, opt => opt.MapFrom(src => src.Answers));
            CreateMap<QuestionDTO, Question>()
               .ForMember(dest => dest.UserQuestions, opt => opt.Ignore()).ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.QuestionAnswers));



            CreateMap<QuestionAnswer, QuestionAnswerDTO>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.Answer, opt => opt.MapFrom(src => src.Answer))
                 .ForMember(dest => dest.AnswerValue, opt => opt.MapFrom(src => src.AnswerValue))
                 .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId)).ForMember(dest => dest.Question, opt => opt.Ignore());


            CreateMap<QuestionAnswerDTO, QuestionAnswer>()
               .ForMember(dest => dest.GivenUserAnswers, opt => opt.Ignore())
               .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question));


            CreateMap<UserModule, UserModuleDTO>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                 .ForMember(dest => dest.UserFirstName, opt => opt.MapFrom(src => src.User.Name))
                 .ForMember(dest => dest.UserLastName, opt => opt.MapFrom(src => src.User.Surname))
                 .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email))
                 .ForMember(dest => dest.UserPicture, opt => opt.MapFrom(src => src.User.Picture))
                 .ForMember(dest => dest.UserPhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))

                 .ForMember(dest => dest.Module, opt => opt.MapFrom(src => new ModuleDTO
                 {
                     Id = src.Module.Id,
                     Title = src.Module.Title,
                     CreatedAt = src.Module.CreatedAt,
                     Category = src.Module.Category
                 })) 
                 .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
                 .ForMember(dest => dest.UserQuestions, opt => opt.MapFrom(src => src.UserQuestions))
                 .ForMember(dest => dest.UserGivenAnswers, opt => opt.MapFrom(src => src.UserGivenAnswers));
            CreateMap<UserModuleDTO, UserModule>()
              .ForMember(dest => dest.Module, opt => opt.Ignore())
              .ForMember(dest => dest.User, opt => opt.Ignore());


            CreateMap<UserQuestion, UserQuestionDTO>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.UserModuleId, opt => opt.MapFrom(src => src.UserModuleId))
               .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
               .ForMember(dest => dest.IsVisible, opt => opt.MapFrom(src => src.IsVisible))
               .ForMember(dest => dest.QuestionTitle, opt => opt.MapFrom(src => src.Question.QuestionTitle))
               .ForMember(dest => dest.QuestionType, opt => opt.MapFrom(src => src.Question.QuestionType))
               .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Question.Answers))
               .ForMember(dest => dest.QuestionPriority, opt => opt.MapFrom(src => (QuestionPriority)src.QuestionPriority));
            CreateMap<UserQuestionDTO, UserQuestion>()
            .ForMember(dest => dest.Question, opt => opt.Ignore())
            .ForMember(dest => dest.UserModule, opt => opt.Ignore());


            CreateMap<CreateSettingCommand, UserQuestion>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.Question, opt => opt.Ignore())
                .ForMember(dest => dest.UserModule, opt => opt.Ignore());


            CreateMap<UserAnswer, UserAnswerDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserModuleId, opt => opt.MapFrom(src => src.UserModule.Id))
                .ForMember(dest => dest.QuestionAnswerId, opt => opt.MapFrom(src => src.QuestionAnswer.Id))
                .ForMember(dest => dest.GivenAnswer, opt => opt.MapFrom(src => src.GivenAnswer));
            CreateMap<UserAnswerDTO, UserAnswer>();
        }
    }
}
