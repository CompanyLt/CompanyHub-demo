namespace Darbu_records.Query.Department
{
    public class CreateDepartmentQueryBuilder : IDepartmentQueryBuilder
    {

        private readonly CreateDepartmentQuery _createDepartmentQuery;
        public CreateDepartmentQueryBuilder(CreateDepartmentQuery createDepartmentQuery) 
        {
        _createDepartmentQuery = createDepartmentQuery;
        
        }



        public void SetQuery()
            {
                _createDepartmentQuery.SetMainTopic();
                _createDepartmentQuery.SetNavigationActionAccess();
                _createDepartmentQuery.SetFileQuery();
            }



        public string GetMainTopic()=>
            _createDepartmentQuery.GetMainTopic();
        

        public string GetNavigationAction()
        {
            throw new NotImplementedException();
        }

        public string GetNavigationActionAccess()=>
            _createDepartmentQuery.GetNavigationActionAccess();

        public string GetFileQuery()=>
            _createDepartmentQuery.GetFileQuery();
       

    
    }
}
