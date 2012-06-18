namespace RunningObjects.MVC.Security.Policies
{
    public class DenyAllSecurityPolicy : ISecurityPolicy
    {
        #region Implementation of ISecurityPolicy

        public bool Authorize(SecurityPolicyContext context)
        {
            return false;
        }

        #endregion
    }
}