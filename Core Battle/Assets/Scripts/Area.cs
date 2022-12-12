using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace tutoriales
{
    public class Area : MonoBehaviour
    {
        private static Area _area;
        public static Area AREA()
        {
            if (_area == null)
            {
                _area = FindObjectOfType<Area>();
            }
            return _area;
        }


        public MeshRenderer Cylinder;

        public AreaStep[] Steps;
        public float timeElapsed;
        public AreaStep actualArea;

        int step = 0;
        private Transform mTransform;

        private void OnEnable()
        {
            mTransform = this.transform;
        }

        void NextStep()
        {
            if(step < Steps.Length)
            {
                AreaStep preArea = actualArea;
                actualArea = Steps[step++];
                actualArea.RandomPos(preArea);
            }
        }

        private void FixedUpdate()
        {
            timeElapsed += Time.deltaTime;

            if(actualArea.CheckStart(timeElapsed))
            {
                if (step == 0)
                {
                    Cylinder.enabled = true;
                }

                float timePercent = actualArea.EndPercentage(timeElapsed);

                if(timePercent >= 1)
                {
                    NextStep();
                }
                else
                {
                    mTransform.position = Vector3.Lerp(mTransform.position, actualArea.position, timePercent * Time.deltaTime);
                    mTransform.localScale = Vector3.Lerp(mTransform.lossyScale, new Vector3(actualArea.Radius, mTransform.localScale.y, actualArea.Radius), timePercent * Time.deltaTime);
                }
            }

        }

    }

    [System.Serializable]
    public class AreaStep
    {
        public Vector3 position;
        public float Radius;
        public float timeStart;
        public float timeEnd;
        public float Damage;
        public float DamageFrequency;

        public void RandomPos(AreaStep prevStep)
        {
            Vector3 pos = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            pos.Normalize();

            float nRadius = this.Radius / 2;
            pos *= nRadius;

            Vector3 finalPos = prevStep.position + pos;
            this.position = finalPos;
        }

        public bool CheckStart(float timeElapsed)
        {
            return timeElapsed >= timeStart;
        }

        public float EndPercentage(float timeElapsed)
        {
            float timeDifference = timeEnd - timeStart;
            float timeStep = timeElapsed - timeStart;

            return timeStep / timeDifference;
        }

        public bool MustDamage(float timeElapsed)
        {
            return timeElapsed >= DamageFrequency;
        }
    }
}

