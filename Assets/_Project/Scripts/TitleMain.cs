using System;
using System.Collections;
using System.Collections.Generic;
using Com.MyCompany.MyGame.Common;
using Com.MyCompany.MyGame.Title;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace Com.MyCompany.MyGame
{
    public class TitleMain : MonoBehaviour
    {
        private async void Start()
        {
            await RunAllTasks();
            SceneManager.LoadScene("Launcher");
        }

        private async UniTask RunAllTasks()
        {
            await UniTask.NextFrame();
            var types = InheritHelper.GetAllImplementations<ITitleTask>();
            List<ITitleTask> tasks = ListPool<ITitleTask>.Get();

            foreach (var type in types)
            {
                var titleTask = (ITitleTask)Activator.CreateInstance(type);
                if (titleTask == null) continue;
                tasks.Add(titleTask);
            }

            for(var priority = ITitleTaskPriority.High; priority <= ITitleTaskPriority.Low; priority++)
            {
                var p = priority;
                List<ITitleTask> subTasks = ListPool<ITitleTask>.Get();
                var titleTasks = tasks.FindAll(x => x.Priority == p);
                if(titleTasks.Count == 0) continue;
                await RunSubTasks(titleTasks);
                ListPool<ITitleTask>.Release(subTasks);
            }

            ListPool<ITitleTask>.Release(tasks);
        }

        private async UniTask RunSubTasks(IEnumerable<ITitleTask> subTasks)
        {
            foreach (var titleTask in subTasks)
            {
                titleTask.Initialize(OnProgress);
            }
            await UniTask.WhenAll(subTasks.Select(x => x.RunTask()));
        }

        public void OnProgress(int hashCode, float progress)
        {
            Debug.Log($"Progress: {hashCode} {progress}");
        }
    }
}