using AutoMapper;
using EmployeeManagement.Domain;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Employees.Commands;

namespace EmployeeManagement.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<string, string>().ConvertUsing((src,_) => src ?? string.Empty);
            CreateMap<Employee, EmployeeResponse>();
            
            CreateMap<CreateEmployeeRequest, CreateEmployeeCommand>();
            
            CreateMap<CreateEmployeeCommand, Employee>();
            CreateMap<UpdateEmployeeCommand, Employee>();
        }
    }
}