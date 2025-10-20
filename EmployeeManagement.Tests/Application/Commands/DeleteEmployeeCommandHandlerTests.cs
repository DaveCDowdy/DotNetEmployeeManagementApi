using Moq;
using Xunit;
using FluentAssertions;
using EmployeeManagement.Domain;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.Employees.Commands;

namespace EmployeeManagement.Tests.Application.Commands
{
    public class DeleteEmployeeCommandHandlerTests
    {
        private readonly Mock<IEmployeeRepository> _mockRepo;
        private readonly DeleteEmployeeCommandHandler _handler;

        public DeleteEmployeeCommandHandlerTests()
        {
            _mockRepo = new Mock<IEmployeeRepository>();
            _handler = new DeleteEmployeeCommandHandler(_mockRepo.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteEmployee_WhenEmployeeExists()
        {
            const int employeeId = 5;
            
            var existingEmployee = new Employee(
                Id: employeeId, 
                FirstName: "To", 
                LastName: "BeDeleted", 
                Email: "delete@me.com", 
                Phone: "111-0000", 
                Position: "Temp"
            );

            var command = new DeleteEmployeeCommand(employeeId);
            
            _mockRepo.Setup(r => r.GetByIdAsync(employeeId)).ReturnsAsync(existingEmployee);
            
            _mockRepo.Setup(r => r.DeleteAsync(employeeId)).Returns(Task.CompletedTask);
            
            var act = async () => await _handler.Handle(command, CancellationToken.None);
            
            await act.Should().NotThrowAsync();
            
            _mockRepo.Verify(r => r.GetByIdAsync(employeeId), Times.Once, "The handler must fetch the existing entity first.");
            _mockRepo.Verify(r => r.DeleteAsync(employeeId), Times.Once, "The repository's DeleteAsync must be called once with the correct ID.");
        }
        
        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenEmployeeDoesNotExist()
        {
            const int nonExistentId = 99;
            
            var command = new DeleteEmployeeCommand(nonExistentId);
            
            Employee? employee = null;
            _mockRepo.Setup(r => r.GetByIdAsync(nonExistentId)).ReturnsAsync(employee);
            
            var act = async () => await _handler.Handle(command, CancellationToken.None);
            
            await act.Should().ThrowExactlyAsync<KeyNotFoundException>()
                     .WithMessage($"Employee with ID {nonExistentId} not found for deletion.");
            
            _mockRepo.Verify(r => r.GetByIdAsync(nonExistentId), Times.Once, "The handler must attempt to fetch the entity.");
            _mockRepo.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never, "DeleteAsync should never be called if the entity doesn't exist.");
        }
    }
}