using Moq;
using Xunit;
using FluentAssertions;
using AutoMapper;
using EmployeeManagement.Domain;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Employees.Commands;

namespace EmployeeManagement.Tests.Application.Commands
{
    public class CreateEmployeeCommandHandlerTests
    {
        private readonly Mock<IEmployeeRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CreateEmployeeCommandHandler _handler;

        public CreateEmployeeCommandHandlerTests()
        {
            _mockRepo = new Mock<IEmployeeRepository>();
            _mockMapper = new Mock<IMapper>();

            _handler = new CreateEmployeeCommandHandler(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateEmployeeAndReturnResponse()
        {
            var command = new CreateEmployeeCommand
            {
                FirstName = "Lois",
                LastName = "Lane",
                Email = "lois@daily.com",
                Phone = "555-2222",
                Position = "Senior Reporter"
            };
            
            var domainEmployeeBeforeSave = new Employee(
                0, 
                command.FirstName,
                command.LastName,
                command.Email,
                command.Phone,
                command.Position
            );

            const int newEmployeeId = 10;
            var domainEmployeeAfterSave = new Employee(
                newEmployeeId, 
                command.FirstName,
                command.LastName,
                command.Email,
                command.Phone,
                command.Position
            );
            
            var expectedResponse = new EmployeeResponse(
                Id: newEmployeeId,
                FirstName: command.FirstName,
                LastName: command.LastName,
                Email: command.Email,
                Phone: command.Phone,
                Position: command.Position
            );

            _mockMapper.Setup(m => m.Map<Employee>(command))
                .Returns(domainEmployeeBeforeSave);

            _mockRepo.Setup(r => r.AddAsync(domainEmployeeBeforeSave))
                .ReturnsAsync(domainEmployeeAfterSave);

            _mockMapper.Setup(m => m.Map<EmployeeResponse>(domainEmployeeAfterSave))
                .Returns(expectedResponse);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResponse);

            _mockMapper.Verify(m => m.Map<Employee>(command), Times.Once,
                "Mapper must convert the command to a domain entity.");
            _mockRepo.Verify(r => r.AddAsync(domainEmployeeBeforeSave), Times.Once,
                "Repository must be called once to save the new employee.");
            _mockMapper.Verify(m => m.Map<EmployeeResponse>(domainEmployeeAfterSave), Times.Once,
                "Mapper must convert the saved entity back to the DTO.");
        }
    }
}