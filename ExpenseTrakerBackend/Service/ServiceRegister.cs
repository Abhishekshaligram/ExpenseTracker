
using PracticeCrud.Service.Budget;
using PracticeCrud.Service.Category;
using PracticeCrud.Service.Expense;
using PracticeCrud.Service.General;
using PracticeCrud.Service.Login;
using PracticeCrud.Service.User;

namespace PracticeCrud.Service
{
    public class ServiceRegister
    {

        public static Dictionary<Type, Type> GetTypes()
        {
            var serviceDictionary = new Dictionary<Type, Type>
            {
                {typeof(IEmployeeService), typeof(EmployeeService) },
                 {typeof(IUserService), typeof(UserService) },
                 {typeof(IExpenseService), typeof(ExpenseService) },
                  {typeof(ICategoryService), typeof(CategoryService) },
                 {typeof(IGeneralService), typeof(GeneralService) },
                  {typeof(ILoginService), typeof(LoginService) },
                   {typeof(IBudgetService), typeof(BudgetService) },

            };
            return serviceDictionary;

            }

    }
}
