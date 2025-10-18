using AutoMapper;
using EmployeeManagement.Domain;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Employees.Commands;
using EmployeeManagement.Application.Employees.Queries;

namespace EmployeeManagement.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeResponse>();
            
            CreateMap<CreateEmployeeRequest, CreateEmployeeCommand>();
            
            CreateMap<CreateEmployeeCommand, Employee>();
            CreateMap<UpdateEmployeeCommand, Employee>();
        }
    }
}