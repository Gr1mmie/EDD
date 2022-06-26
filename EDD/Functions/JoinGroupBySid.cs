﻿using System;
using System.DirectoryServices.AccountManagement;
using EDD.Models;

namespace EDD.Functions
{
    public class JoinGroupBySid : EDDFunction
    {
        public override string FunctionName => "JoinGroupBySid";

        public override string[] Execute(ParsedArgs args)
        {
            if (args.GroupName == null)
            {
                return new string[] {"You need to provide a SID in the GroupName parameter"};
            }

            if (args.UserName == null)
            {
                return new string[] {"You need to provide a username to join the targeted group"};
            }

            try
            {
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain))
                {
                    GroupPrincipal group = GroupPrincipal.FindByIdentity(pc, IdentityType.Sid, args.GroupName);
                    group.Members.Add(pc, IdentityType.SamAccountName, args.UserName);
                    group.Save();
                }

                return new string[] {"Joined account to group"};
            }
            catch (Exception e)
            {
                return new string[] { "[X] Failure to join user to group - " + e };
            }
        }
    }
}