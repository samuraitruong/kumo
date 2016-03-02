using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test_kumo_eip0001model;
using test_kumo_eip0001repositories;

namespace test_kumo_eip0001application
{
    public class UserActionService : ServiceBase<UserAction>
    {
        private readonly UserActionRepository userActionRepository;
        public UserActionService()
        {
            userActionRepository = new UserActionRepository();
        }

        public List<UserAction> GetUserActions(string email)
        {
            var list = userActionRepository.GetUserActions(email);
            return list;
            var groupded = list.GroupBy(p => p.ComponentId);

            var result = new List<UserAction>();
            foreach (var group in groupded)
            {
                result.AddRange(group);
                result.Add(new UserAction() { 
                    ComponentId = group.Key,
                    ActionId = (int)Actions.View,
                    Component = group.FirstOrDefault().Component
                });
            }
            return result;
        }

        public List<SystemAction> GetSystemActions()
        {
            return userActionRepository.GetSystemActions().ToList();
        }

        public void UpdateUserPermissions(string[] viewPermissions, User user)
        {
            if(viewPermissions != null && viewPermissions.Count() > 0)
            {
                List<UserAction> actions = new List<UserAction>();
                viewPermissions.ToList().ForEach(perm =>
                {
                    if (perm.IndexOf("|") > 0)
                    {
                        var perms = perm.Split('|');
                        actions.Add(new UserAction()
                        {
                            ComponentId = int.Parse(perms[0]),
                            ActionId = int.Parse(perms[1]),
                            UserId = user.Id
                        });
                    }
                });

                var efPermissions = GetUserActions(user.Email);

                var addPermissions = actions.Where(vp => !efPermissions.Any(ef => ef.ActionId == vp.ActionId &&
                    ef.ComponentId == vp.ComponentId && ef.UserId == vp.UserId));
                var deletePermissions = efPermissions.Where(ef => !actions.Any(vp => vp.ComponentId == ef.ComponentId &&
                    vp.ActionId == ef.ActionId && vp.UserId == ef.UserId));

                Add(addPermissions.ToArray());
                Delete(deletePermissions.ToArray());
            }
            
        }

        public List<string> GetUserPermissions(User user)
        {
            List<string> actions = new List<string>();

            var efPermissions = GetUserActions(user.Email);
            efPermissions.ForEach(perm => {
                actions.Add(string.Format("{0}|{1}", perm.ComponentId, perm.ActionId));
            });

            return actions;
        }
    }
}
