namespace EmployeeManagement.Application.Interfaces
{
    public interface IEmployeeCommand
    {
        string FirstName { get;  }
        string LastName { get;  }
        string Email { get;  }
        string Phone { get;  }
        string Position { get;  }
    }
}