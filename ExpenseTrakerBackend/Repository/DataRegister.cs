using PracticeCrud.Repository.Budget;
using PracticeCrud.Repository.Category;
using PracticeCrud.Repository.Expense;
using PracticeCrud.Repository.General;
using PracticeCrud.Repository.Login;
using PracticeCrud.Repository.User;
namespace PracticeCrud.Repository
{
    public class DataRegister
    {
        public static Dictionary<Type, Type> GetTypes(){

            var serviceDictionary = new Dictionary<Type, Type> {

                {typeof(IEmployeeRepository), typeof(EmployeeRepository)},
                {typeof(IUserRepository), typeof(UserRepository)},
                  {typeof(IExpenseRepository), typeof(ExpenseRepository)},
                    {typeof(ICategoryRepository), typeof(CategoryRepository)},
                  {typeof(IGeneralRepository), typeof(GeneralRepository)},
                  {typeof(ILoginRepository), typeof(LoginRepository)},
                    {typeof(IBudgetRepository), typeof(BudgetRepository)},



            };
            return serviceDictionary;

        }

    }
}
