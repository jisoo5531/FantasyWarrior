using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyScene
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance { get; private set; }

        private int _currentSceneNumber = 0;
        private int _nextSceneNumber = 0;
        public int currentSceneNumber
        {
            get
            {
                return _currentSceneNumber;
            }
            set
            {
                _currentSceneNumber = value;
            }
        }

        public int nextSceneNumber
        {
            get
            {
                return _nextSceneNumber;
            }
            set
            {
                _nextSceneNumber = value;
            }
        }
        private void Awake()
        {
            // Singleton 패턴 구현
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // 이 오브젝트가 씬 전환 시에도 파괴되지 않도록 설정
            }
            else
            {
                Destroy(gameObject); // 이미 인스턴스가 존재하면 새로 생성된 오브젝트는 파괴
            }
        }
        public int CurrentMinusNext()
        {
            Debug.Log($"현재 씬 : {_currentSceneNumber}, 다음 씬 : {_nextSceneNumber}");
            return _currentSceneNumber - _nextSceneNumber;
        }
        public void SetSceneNumber(int current, int next)
        {
            
            _currentSceneNumber = current;
            _nextSceneNumber = next;
            Debug.Log($"현재 : {_currentSceneNumber}, 다음 : {_nextSceneNumber}");
        }

        
    }
    public enum Scene
    {
        Start,
        Human,
        Goblin
    }
}


