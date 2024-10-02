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
            // Singleton ���� ����
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // �� ������Ʈ�� �� ��ȯ �ÿ��� �ı����� �ʵ��� ����
            }
            else
            {
                Destroy(gameObject); // �̹� �ν��Ͻ��� �����ϸ� ���� ������ ������Ʈ�� �ı�
            }
        }
        public int CurrentMinusNext()
        {
            Debug.Log($"���� �� : {_currentSceneNumber}, ���� �� : {_nextSceneNumber}");
            return _currentSceneNumber - _nextSceneNumber;
        }
        public void SetSceneNumber(int current, int next)
        {
            
            _currentSceneNumber = current;
            _nextSceneNumber = next;
            Debug.Log($"���� : {_currentSceneNumber}, ���� : {_nextSceneNumber}");
        }

        
    }
    public enum Scene
    {
        Start,
        Human,
        Goblin
    }
}


