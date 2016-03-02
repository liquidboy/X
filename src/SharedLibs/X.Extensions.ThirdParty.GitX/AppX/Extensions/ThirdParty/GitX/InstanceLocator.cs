using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Extensions.ThirdParty.GitX
{
    public sealed class InstanceLocator
    {

        private static volatile InstanceLocator instance;
        private static object syncRoot = new Object();


        public GitHubClient GitClient {get; private set;}


        private InstanceLocator() {
            GitClient = new GitHubClient(new ProductHeaderValue("X"));
        }

        public static InstanceLocator Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new InstanceLocator();
                    }
                }

                return instance;
            }
        }


    }
}
