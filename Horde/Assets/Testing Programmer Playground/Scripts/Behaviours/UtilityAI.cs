using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class UtilityMath
{
    public class SimpleRule
    {
        static float GreaterThan(float pMin, float pMax, float pValue, float pComparisonValue)
        {
            float clampedValue = Mathf.Clamp(pValue, pMin, pMax);
            if (clampedValue > pComparisonValue)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        static float LessThan(float pMin, float pMax, float pValue, float pComparisonValue)
        {
            float clampedValue = Mathf.Clamp(pValue, pMin, pMax);
            if (clampedValue < pComparisonValue)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        static float GreaterThanOrEqual(float pMin, float pMax, float pValue, float pComparisonValue)
        {
            float clampedValue = Mathf.Clamp(pValue, pMin, pMax);
            if (clampedValue >= pComparisonValue)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        static float LessThanOrEqual(float pMin, float pMax, float pValue, float pComparisonValue)
        {
            float clampedValue = Mathf.Clamp(pValue, pMin, pMax);
            if (clampedValue <= pComparisonValue)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        static float Equal(float pMin, float pMax, float pValue, float pComparisonValue)
        {
            float clampedValue = Mathf.Clamp(pValue, pMin, pMax);
            if (clampedValue == pComparisonValue)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        static float NotEqual(float pMin, float pMax, float pValue, float pComparisonValue)
        {
            float clampedValue = Mathf.Clamp(pValue, pMin, pMax);
            if (clampedValue != pComparisonValue)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
    static public float Linear(float pMin, float pMax, float pValue)
    {
        float clampedValue = Mathf.Clamp(pValue, pMin, pMax);
        float range = pMax - pMin;
        return (clampedValue - pMin) / range;
    }
    static public float Exponential(float pMin, float pMax, float pValue, float pPower)
    {
        float clampedValue = Mathf.Clamp(pValue, pMin, pMax);
        return Mathf.Pow(clampedValue, pPower) / Mathf.Pow(pMax, pPower);
    }
    static public float Quadratic(float pMin, float pMax, float pValue, float pA, float pB, float pC)
    {
        float clampedValue = Mathf.Clamp(pValue, pMin, pMax);
        // only works if at least one of the coefficients are not 0
        return pA * Mathf.Pow(clampedValue, 2) + (pB * clampedValue) + pC;
    }
}

public class UtilityAI : MonoBehaviour {

    public enum Evaluations
    {
        eval1,
        eval2,
        eval3
    }

    public enum Behaviours
    {
        behaviour1,
        behaviour2
    }

    public enum ROperator
    {
        None,
        Addition,
        Multiplication,
    }

    [System.Serializable]
    public class Eval
    {
        public Evaluations m_eval;
        public ROperator m_operator = ROperator.None;
    }

    [System.Serializable]
    public class BehaveEval
    {
        public List<Eval> m_evaluations;
        public Behaviours m_behaviour;
    }

    public List<BehaveEval> m_behaviours;

    

    /// <summary>
    /// end
    /// </summary>

    //public List<UtilityBehaviour> m_utilityBehaviours;

    [System.Serializable]
    public class UtilityValue
    {
        public UtilityValue()
            :this(NormalisationFormula.Linear, 0, 1.0f)
        {
        }
        public UtilityValue(NormalisationFormula pType, float pMin, float pMax)
        {
            m_min = pMin;
            m_max = pMax;
            m_value = pMin;
            m_normalisedValue = 0;
            m_normalisationType = pType;
        }
        public enum NormalisationFormula
        {
            SimpleRule,
            InverseSimpleRule,
            Linear,
            InverseLinear,
            Exponential,
            InverseExponential,
            Logistic,
            InverseLogistic,
            Logit,
            InverseLogit,
            Quadratic,
            InverseQuadratic
        }

        // private begin
        public float m_min = 0;
        public float m_max = 0;

        public float m_value = 0;
        public float m_normalisedValue = 0;

        public NormalisationFormula m_normalisationType;
        private float m_exponentialPower = 1.0f;
        private float m_quadraticCoefficientA = 1.0f;
        private float m_quadraticCoefficientB = 1.0f;
        private float m_quadraticCoefficientC = 0.0f;
        //private end



        public void SetMinMaxValues(float pMin, float pMax)
        {
            m_min = pMin;
            m_max = pMax;
        }
        public void SetNormalisationType(NormalisationFormula pType)
        {
            m_normalisationType = pType;
        }
        public void SetValue(float pValue)
        {
            m_value = pValue;
        }
        public void SetExponentialProperties(float pPower)
        {
            m_exponentialPower = pPower;
        }
        public void SetQuadraticCoefficients(float pA, float pB, float pC)
        {
            m_quadraticCoefficientA = pA;
            m_quadraticCoefficientB = pB;
            m_quadraticCoefficientC = pC;
        }
        public float Evaluate()
        {
            m_normalisedValue = 0;
            switch (m_normalisationType)
            {
                //case NormalisationFormula.SimpleRule:
                //    break;
                //case NormalisationFormula.InverseSimpleRule:
                //    break;
                case NormalisationFormula.Linear:
                    m_normalisedValue = UtilityMath.Linear(m_min, m_max, m_value);
                    break;
                case NormalisationFormula.InverseLinear:
                    m_normalisedValue = 1 - UtilityMath.Linear(m_min, m_max, m_value);
                    break;
                case NormalisationFormula.Exponential:
                    m_normalisedValue = UtilityMath.Exponential(m_min, m_max, m_value, m_exponentialPower);
                    break;
                case NormalisationFormula.InverseExponential:
                    m_normalisedValue = 1 - UtilityMath.Exponential(m_min, m_max, m_value, m_exponentialPower);
                    break;
                //case NormalisationFormula.Logistic:
                //    break;
                //case NormalisationFormula.InverseLogistic:
                //    break;
                //case NormalisationFormula.Logit:
                //    break;
                //case NormalisationFormula.InverseLogit:
                //    break;
                case NormalisationFormula.Quadratic:
                    m_normalisedValue = UtilityMath.Quadratic(m_min, m_max, m_value, m_quadraticCoefficientA, m_quadraticCoefficientB, m_quadraticCoefficientC);
                    break;
                case NormalisationFormula.InverseQuadratic:
                    m_normalisedValue = 1 - UtilityMath.Quadratic(m_min, m_max, m_value, m_quadraticCoefficientA, m_quadraticCoefficientB, m_quadraticCoefficientC);
                    break;
                default:
                    Debug.Log("No matching Normalisation Formula for " + m_normalisationType.ToString());
                    break;
            }

            m_normalisedValue = Mathf.Clamp(m_normalisedValue, 0, 1.0f);
            return m_normalisedValue;
        }
    }

    [System.Serializable]
    public class UtilityInfo/* : UtilityNode*/
    {
        public UtilityInfo()
            : this(new UtilityValue(), 1.0f)
        {
        }
        public UtilityInfo(UtilityValue pValue, float pModifier)
        {
            m_value = pValue;
            m_modifier = pModifier;
        }
        public UtilityValue m_value;
        public float m_modifier; // weight of the score
        public /*override*/ virtual float Evaluate()
        {
            return m_value.Evaluate() * m_modifier;
        }
    }

    [System.Serializable]
    public class UtilityCombination : UtilityInfo
    {

        public enum OperatorType
        {
            Addition,
            Multiplication,
            //Subtraction,
            //Division,
        }
        public OperatorType m_operator = OperatorType.Multiplication;
        //private
        //public UtilityInfo m_utilityA = null;
        //public void SetUtilityA(UtilityValue pValue, float pModifier)
        //{
        //    m_utilityA = new UtilityInfo(pValue, pModifier);
        //}
        //public void SetUtilityA(UtilityInfo pInfo)
        //{
        //    m_utilityA = pInfo;
        //}


        public UtilityInfo m_utilityB = null;

        public void SetUtilityB(UtilityValue pValue, float pModifier)
        {
            m_utilityB = new UtilityInfo(pValue, pModifier);
        }

        public void SetUtilityB(UtilityInfo pInfo)
        {
            m_utilityB = pInfo;
        }


        //public float GetUtilityScore()
        //{
        //    float score = 0.0f;
        //    if (m_utilityA != null)
        //    {
        //        score = m_utilityA.Evaluate();
        //        if (m_utilityB != null)
        //        {
        //            switch (m_operator)
        //            {
        //                case OperatorType.Addition:
        //                    score += m_utilityB.Evaluate();
        //                    break;
        //                case OperatorType.Multiplication:
        //                    score *= m_utilityB.Evaluate();
        //                    break;
        //                //case OperatorType.Subtraction:
        //                //    score -= m_utilityB.m_value.Evaulate() * m_utilityB.m_modifier;
        //                //    break;
        //                //case OperatorType.Division:
        //                //    score /= m_utilityB.m_value.Evaulate() * m_utilityB.m_modifier;
        //                //    break;
        //                default:
        //                    break;
        //            }
        //        }
        //    }
        //    return score;
        //}

        public float GetUtilityScore()
        {
            float score = 0.0f;
            score = base.Evaluate();
            if (m_utilityB != null)
            {
                switch (m_operator)
                {
                    case OperatorType.Addition:
                        score += m_utilityB.Evaluate();
                        break;
                    case OperatorType.Multiplication:
                        score *= m_utilityB.Evaluate();
                        break;
                    //case OperatorType.Subtraction:
                    //    score -= m_utilityA.m_value.Evaulate() * m_utilityA.m_modifier;
                    //    break;
                    //case OperatorType.Division:
                    //    score /= m_utilityA.m_value.Evaulate() * m_utilityA.m_modifier;
                    //    break;
                    default:
                        break;
                }
            }
            return score;
        }


        public override float Evaluate()
        {
            return GetUtilityScore();
        }
    }

    




    // Use this for initialization
    void Start ()
    {
        //m_info = new UtilityInfo();
        //m_combo = new UtilityCombination();
        //m_combo.SetUtilityB(new UtilityValue(), 1.0f);
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    //[System.Serializable]
    //public class UtilityBehaviour
    //{
    //    public List<string> m_information;
    //    public BaseBehaviour m_behaviour;
    //}

    //[System.Serializable]
    //public class UtilityNode/*: MonoBehaviour*/
    //{
    //    public string m_identifier = "test";
    //    public virtual float Evaluate() { return 0; }
    //    //public virtual float Evaluation { get { return 0; } }
    //}


    //public UtilityInfo m_info;
    //public UtilityCombination m_combo;
}


//[CustomEditor(typeof(UtilityCombination))]
//public class MyScriptEditor : Editor
//{
//    void OnInspectorGUI()
//    {
//        var myScript = target as MyScript;

//        myScript.flag = GUILayout.Toggle(myScript.flag, "Flag");

//        if (myScript.flag)
//            myScript.i = EditorGUILayout.IntSlider("I field:", myScript.i, 1, 100);

//    }
//}

