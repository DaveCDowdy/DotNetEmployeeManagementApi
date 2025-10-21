using Moq;
using Xunit;
using FluentAssertions;
using AutoMapper;
using EmployeeManagement.Domain;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.Employees.Commands;

namespace EmployeeManagement.Tests.Application.Commands
{
    public class UpdateEmployeeCommandHandlerTests
    {
        private readonly Mock<IEmployeeRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UpdateEmployeeCommandHandler _handler;

        public UpdateEmployeeCommandHandlerTests()
        {
            _mockRepo = new Mock<IEmployeeRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new UpdateEmployeeCommandHandler(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateEmployee_WhenEmployeeExists()
        {
            const int employeeId = 5;
    
            var command = new UpdateEmployeeCommand
            {
                Id = employeeId,
                FirstName = "NewName",
                LastName = "NewLast",
                Email = "new@corp.com",
                Phone = "111-2222",
                Position = "Senior Dev"
            };
            
            var existingEmployee = new Employee(
                employeeId,     
                "OldName", 
                "OldLast", 
                "old@corp.com", 
                "000-0000", 
                "Junior Dev"
            );
    
            _mockRepo.Setup(r => r.GetByIdAsync(employeeId)).ReturnsAsync(existingEmployee);
    
            _mockMapper.Setup(m => m.Map(command, existingEmployee)); 
    
            _mockRepo.Setup(r => r.UpdateAsync(existingEmployee)).Returns(Task.CompletedTask);
    
            var act = async () => await _handler.Handle(command, CancellationToken.None);
    
            await act.Should().NotThrowAsync();
    
            _mockRepo.Verify(r => r.GetByIdAsync(employeeId), Times.Once, "The handler must fetch the existing entity.");
            _mockMapper.Verify(m => m.Map(command, existingEmployee), Times.Once, "The mapper must be called to apply changes to the existing entity.");
            _mockRepo.Verify(r => r.UpdateAsync(existingEmployee), Times.Once, "The repository must be called to save the modified entity.");
        }
        
        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenEmployeeDoesNotExist()
        {
            const int nonExistentId = 99;
            
            var command = new UpdateEmployeeCommand { Id = nonExistentId };
            
            Employee? employee = null;
            _mockRepo.Setup(r => r.GetByIdAsync(nonExistentId)).ReturnsAsync(employee);
            
            var act = async () => await _handler.Handle(command, CancellationToken.None);
            
            await act.Should().ThrowExactlyAsync<KeyNotFoundException>()
                     .WithMessage($"Employee with ID {nonExistentId} not found.");
            
            _mockRepo.Verify(r => r.GetByIdAsync(nonExistentId), Times.Once, "The handler must attempt to fetch the entity.");
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Employee>()), Times.Never, "UpdateAsync should never be called if the entity doesn't exist.");
            _mockMapper.Verify(m => m.Map(It.IsAny<UpdateEmployeeCommand>(), It.IsAny<Employee>()), Times.Never, "Mapper should not be called if the entity doesn't exist.");
        }
    }
}