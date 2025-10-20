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
    public class GetAllEmployeesQueryHandlerTests
    {
        private readonly Mock<IEmployeeRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetAllEmployeesQueryHandler _handler;
        
        public GetAllEmployeesQueryHandlerTests()
        {
            _mockRepo = new Mock<IEmployeeRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetAllEmployeesQueryHandler(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmployees_WhenEmployeesExist()
        {
          
            var domainEmployees = new List<Employee>
            {
                new(1, "John", "Doe", "john.d@co.com", "555-0101", "Developer"),
                new(2, "Jane", "Smith", "jane.s@co.com", "555-0202", "Architect")
            };
            
            var expectedResponses = new[] 
            {
                new EmployeeResponse(1, "John", "Doe", "john.d@co.com", "555-0101", "Developer"),
                new EmployeeResponse(2, "Jane", "Smith", "jane.s@co.com", "555-0202", "Architect")
            };
            
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(domainEmployees);
            _mockMapper.Setup(m => m.Map<IEnumerable<EmployeeResponse>>(It.IsAny<IEnumerable<Employee>>()))
                       .Returns(expectedResponses); 

            var query = new GetAllEmployeesQuery();
            
            var resultEnumerable = await _handler.Handle(query, CancellationToken.None);
            
            var result = resultEnumerable.ToList();
            
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResponses);
            result.Should().HaveCount(2);
            
            _mockRepo.Verify(r => r.GetAllAsync(), Times.Once);
            _mockMapper.Verify(m => m.Map<IEnumerable<EmployeeResponse>>(It.IsAny<IEnumerable<Employee>>()), Times.Once);
        }
        
        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoEmployeesExist()
        {
            var domainEmployees = Array.Empty<Employee>();
            var expectedResponses = Array.Empty<EmployeeResponse>();
            
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(domainEmployees);
            _mockMapper.Setup(m => m.Map<IEnumerable<EmployeeResponse>>(It.IsAny<IEnumerable<Employee>>()))
                       .Returns(expectedResponses);
            
            var query = new GetAllEmployeesQuery();
            
            var resultEnumerable = await _handler.Handle(query, CancellationToken.None);
            var result = resultEnumerable.ToList();
            
            result.Should().NotBeNull();
            result.Should().BeEmpty();
            
            _mockRepo.Verify(r => r.GetAllAsync(), Times.Once);
            _mockMapper.Verify(m => m.Map<IEnumerable<EmployeeResponse>>(It.IsAny<IEnumerable<Employee>>()), Times.Once);
        }
    }
}