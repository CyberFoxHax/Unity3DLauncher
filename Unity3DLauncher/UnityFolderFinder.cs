using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Unity3DLauncher
{
    public class UnityFolderFinder : IEnumerable<string> {
        public UnityFolderFinder(string rootPath) {
            _rootPath = rootPath;
        }

        public int IterationsCounter { get; private set; }
        private int _currentDepth;
        private readonly string _rootPath;
        private static readonly string[] UnityFolders = { "Assets", "ProjectSettings"};
        private static readonly string[] SvnFolderes = { "branches", "tags", "trunk"};

        public IEnumerator<string> GetEnumerator() {
            return IterateChildren(_rootPath).GetEnumerator();
        }

        private IEnumerable<string> IterateChildren(string path) {
            _currentDepth++;
            foreach (var d in Directory.EnumerateDirectories(path)) {
                if (d.EndsWith(".svn"))
                    continue;
                IterationsCounter++;
                var subdirs = Directory.EnumerateDirectories(d).ToArray();

                var isUnityFolder = subdirs.Select(Path.GetFileName).Count(p => UnityFolders.Contains(p)) == 2;
                var isSvnFolder   = subdirs.Select(Path.GetFileName).Count(p => SvnFolderes .Contains(p)) == 3;
                if (isUnityFolder)
                    yield return d;
                else if(_currentDepth < 2 || isSvnFolder || SvnFolderes.Any(p=>d.EndsWith(p)))
                    foreach (var iterateChild in IterateChildren(d))
                        yield return iterateChild;
            }
            _currentDepth--;
        }

        IEnumerator IEnumerable.GetEnumerator(){
            return GetEnumerator();
        }
    }
}