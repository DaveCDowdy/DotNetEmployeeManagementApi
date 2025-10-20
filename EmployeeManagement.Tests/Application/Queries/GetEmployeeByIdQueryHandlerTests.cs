using Moq;
using Xunit;
using FluentAssertions;
using AutoMapper;
using EmployeeManagement.Domain;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Employees.Queries;

namespace EmployeeManagement.Tests.Application.Queries
{
    public class GetEmployeeByIdQueryHandlerTests
    {
        private readonly Mock<IEmployeeRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetEmployeeByIdQueryHandler _handler;
        
        public GetEmployeeByIdQueryHandlerTests()
        {
            _mockRepo = new Mock<IEmployeeRepository>();
            _mockMapper = new Mock<IMapper>();
            
            _handler = new GetEmployeeByIdQueryHandler(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmployeeResponse_WhenEmployeeExists()
        {
            const int employeeId = 5;
            
            var domainEmployee = new Employee(
                Id: employeeId, 
                FirstName: "Clark", 
                LastName: "Kent", 
                Email: "clark@daily.com", 
                Phone: "555-1111", 
                Position: "Reporter"
            );
            
            var expectedResponse = new EmployeeResponse(
                Id: employeeId, 
                FirstName: "Clark", 
                LastName: "Kent", 
                Email: "clark@daily.com", 
                Phone: "555-1111", 
                Position: "Reporter"
            );
            
            _mockRepo.Setup(r => r.GetByIdAsync(employeeId)).ReturnsAsync(domainEmployee);
            
            _mockMapper.Setup(m => m.Map<EmployeeResponse>(domainEmployee))
                       .Returns(expectedResponse);

            var query = new GetEmployeeByIdQuery(employeeId);
            
            var result = await _handler.Handle(query, CancellationToken.None);
            
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResponse);
            
            _mockRepo.Verify(r => r.GetByIdAsync(employeeId), Times.Once);
            _mockMapper.Verify(m => m.Map<EmployeeResponse>(domainEmployee), Times.Once);
        }
        
        [Fact]
        public async Task Handle_ShouldReturnNull_WhenEmployeeDoesNotExist()
        {
            const int nonExistentId = 99;
            
            Employee? domainEmployee = null;
            EmployeeResponse? expectedResponse = null;
            
            _mockRepo.Setup(r => r.GetByIdAsync(nonExistentId)).ReturnsAsync(domainEmployee);
            
            _mockMapper.Setup(m => m.Map<EmployeeResponse>(domainEmployee))
                .Returns(expectedResponse);

            var query = new GetEmployeeByIdQuery(nonExistentId);
            
            var result = await _handler.Handle(query, CancellationToken.None);
            
            result.Should().BeNull();
            
            _mockRepo.Verify(r => r.GetByIdAsync(nonExistentId), Times.Once);
            _mockMapper.Verify(m => m.Map<EmployeeResponse>(domainEmployee), Times.Once);
        }
    }
}