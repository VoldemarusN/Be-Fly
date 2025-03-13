using Plane;
using UnityEngine;
using Zenject;

//класс описывает поведение стрелки-указателя которая отвечает за направление запуска самолета
//так же отвечает за запуск по кнопке G
namespace UI
{
    public class DirectionIndicator: MonoBehaviour
    {
        [Inject]
        private PlaneController _planeController;
   
        private float _directionZ;
        [SerializeField] private float _maxAngle, _minAngle;
        [SerializeField] private float _speed;
        [SerializeField] private float _startArrowAngle;
        [SerializeField] private GameObject _arrow;

        private bool _rotating = true;
        private Vector3 _targetEuler;
        // private Vector3 _arrowAngle;

        private void Start(){
            _arrow.transform.localEulerAngles = new Vector3(0f, 0f, _startArrowAngle);
            _targetEuler = new Vector3(0f, 0f, _maxAngle);
        }


        private void Update(){
            if (Input.GetKeyDown(KeyCode.G)){
                StopArrow();
        
                Destroy(this);
            }
            if (_rotating == false) return;
            RotateArrow();
        }


        void RotateArrow(){
            _arrow.transform.localEulerAngles = Vector3.MoveTowards(_arrow.transform.localEulerAngles, _targetEuler, _speed * Time.deltaTime);
            if (_arrow.transform.localEulerAngles == _targetEuler){
                ChangeTarget();
            }
        }

        void ChangeTarget(){
            if (_targetEuler.z == _maxAngle){
                _targetEuler.z = _minAngle;
            }
            else{
                _targetEuler.z = _maxAngle;
            }
        }
        private void StopArrow(){
            _rotating = false;
            _directionZ = _arrow.transform.localEulerAngles.z;
        }
    
    

    }
}
