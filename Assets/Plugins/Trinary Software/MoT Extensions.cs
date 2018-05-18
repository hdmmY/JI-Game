using UnityEngine;

namespace MovementEffects.Extensions
{
    public static class MovementOverTimeExtensionFunctions
    {
        public static Sequence<Transform, Vector3> MoTWorldSpaceMoveTo (this Transform transform, float duration, Vector3 target)
        {
            return new Sequence<Transform, Vector3>
            {
                Effects = new []
                {
                    new Effect<Transform, Vector3>
                    {
                    Duration = duration,
                    RetrieveEnd = _ => target,
                    OnUpdate = (tform, val) => tform.position = val
                    }
                    },
                    Reference = transform,
                    RetrieveSequenceStart = tform => tform.position
            };
        }

        public static Sequence<Transform, Vector3> MoTWorldSpaceMoveTo (this Transform transform, float duration, System.Func<Transform, Vector3> getTarget)
        {
            return new Sequence<Transform, Vector3>
            {
                Effects = new []
                {
                    new Effect<Transform, Vector3>
                    {
                    Duration = duration,
                    RetrieveEnd = getTarget,
                    OnUpdate = (tform, val) => tform.position = val
                    }
                    },
                    Reference = transform,
                    RetrieveSequenceStart = tform => tform.position
            };
        }

        public static Sequence<Transform, Vector3> MoTLocalSpaceMoveTo (this Transform transform, float duration, Vector3 target)
        {
            return new Sequence<Transform, Vector3>
            {
                Effects = new []
                {
                    new Effect<Transform, Vector3>
                    {
                    Duration = duration,
                    RetrieveEnd = _ => target,
                    OnUpdate = (tform, val) => tform.localPosition = val
                    }
                    },
                    Reference = transform,
                    RetrieveSequenceStart = tform => tform.localPosition
            };
        }

        public static Sequence<Transform, Vector3> MoTLocalSpaceMoveTo (this Transform transform, float duration, System.Func<Transform, Vector3> getTarget)
        {
            return new Sequence<Transform, Vector3>
            {
                Effects = new []
                {
                    new Effect<Transform, Vector3>
                    {
                    Duration = duration,
                    RetrieveEnd = getTarget,
                    OnUpdate = (tform, val) => tform.localPosition = val
                    }
                    },
                    Reference = transform,
                    RetrieveSequenceStart = tform => tform.localPosition
            };
        }

        public static Sequence<Transform, Quaternion> MoTWorldSpaceRotateTo (this Transform transform, float duration, Quaternion target)
        {
            return new Sequence<Transform, Quaternion>
            {
                Effects = new []
                {
                    new Effect<Transform, Quaternion>
                    {
                    Duration = duration,
                    RetrieveEnd = _ => target,
                    OnUpdate = (tform, val) => tform.rotation = val
                    }
                    },
                    Reference = transform,
                    RetrieveSequenceStart = tform => tform.rotation
            };
        }

        public static Sequence<Transform, Quaternion> MoTWorldSpaceRotateTo (this Transform transform, float duration,
            System.Func<Transform, Quaternion> getTarget)
        {
            return new Sequence<Transform, Quaternion>
            {
                Effects = new []
                {
                    new Effect<Transform, Quaternion>
                    {
                    Duration = duration,
                    RetrieveEnd = getTarget,
                    OnUpdate = (tform, val) => tform.rotation = val
                    }
                    },
                    Reference = transform,
                    RetrieveSequenceStart = tform => tform.rotation
            };
        }

        public static Sequence<Transform, Quaternion> MoTLocalSpaceRotateTo (this Transform transform, float duration, Quaternion target)
        {
            return new Sequence<Transform, Quaternion>
            {
                Effects = new []
                {
                    new Effect<Transform, Quaternion>
                    {
                    Duration = duration,
                    RetrieveEnd = _ => target,
                    OnUpdate = (tform, val) => tform.localRotation = val
                    }
                    },
                    Reference = transform,
                    RetrieveSequenceStart = tform => tform.localRotation
            };
        }

        public static Sequence<Transform, Quaternion> MoTLocalSpaceRotateTo (this Transform transform, float duration,
            System.Func<Transform, Quaternion> getTarget)
        {
            return new Sequence<Transform, Quaternion>
            {
                Effects = new []
                {
                    new Effect<Transform, Quaternion>
                    {
                    Duration = duration,
                    RetrieveEnd = getTarget,
                    OnUpdate = (tform, val) => tform.localRotation = val
                    }
                    },
                    Reference = transform,
                    RetrieveSequenceStart = tform => tform.localRotation
            };
        }

        public static Sequence<Transform, Vector3> MoTScaleTo (this Transform transform, float duration, Vector3 target)
        {
            return new Sequence<Transform, Vector3>
            {
                Effects = new []
                {
                    new Effect<Transform, Vector3>
                    {
                    Duration = duration,
                    RetrieveEnd = _ => target,
                    OnUpdate = (tform, val) => tform.localScale = val
                    }
                    },
                    Reference = transform,
                    RetrieveSequenceStart = tform => tform.localScale
            };
        }

        public static Sequence<Transform, Vector3> MoTScaleTo (this Transform transform, float duration, System.Func<Transform, Vector3> getTarget)
        {
            return new Sequence<Transform, Vector3>
            {
                Effects = new []
                {
                    new Effect<Transform, Vector3>
                    {
                    Duration = duration,
                    RetrieveEnd = getTarget,
                    OnUpdate = (tform, val) => tform.localScale = val
                    }
                    },
                    Reference = transform,
                    RetrieveSequenceStart = tform => tform.localScale
            };
        }
    }

    public class Easing
    {
        public enum EaseType
        {
            Linear,
            Step,
            SineIn,
            SineOut,
            SineInOut,
            Pow2In,
            Pow2Out,
            Pow2InOut,
            Pow3In,
            Pow3Out,
            Pow3InOut,
            Pow4In,
            Pow4Out,
            Pow4InOut,
            Pow5In,
            Pow5Out,
            Pow5InOut,
            ElasticIn,
            ElasticOut,
            ElasticInOut,
            BounceIn,
            BounceOut,
            BounceInOut,
            BackIn,
            BackOut,
            BackInOut
        }

        public static System.Func<float, float> GetEase (EaseType easeType)
        {
            switch (easeType)
            {
                case EaseType.Linear:
                    return Linear;
                case EaseType.Step:
                    return Step;
                case EaseType.SineIn:
                    return SineIn;
                case EaseType.SineOut:
                    return SineOut;
                case EaseType.SineInOut:
                    return SineInOut;
                case EaseType.Pow2In:
                    return Pow2In;
                case EaseType.Pow2Out:
                    return Pow2Out;
                case EaseType.Pow2InOut:
                    return Pow2InOut;
                case EaseType.Pow3In:
                    return Pow3In;
                case EaseType.Pow3Out:
                    return Pow3Out;
                case EaseType.Pow3InOut:
                    return Pow3InOut;
                case EaseType.Pow4In:
                    return Pow4In;
                case EaseType.Pow4Out:
                    return Pow4Out;
                case EaseType.Pow4InOut:
                    return Pow4InOut;
                case EaseType.Pow5In:
                    return Pow5In;
                case EaseType.Pow5InOut:
                    return Pow5InOut;
                case EaseType.ElasticIn:
                    return ElasticIn;
                case EaseType.ElasticOut:
                    return ElasticOut;
                case EaseType.ElasticInOut:
                    return ElasticInOut;
                case EaseType.BounceIn:
                    return BounceIn;
                case EaseType.BounceOut:
                    return BounceOut;
                case EaseType.BounceInOut:
                    return BounceInOut;
                case EaseType.BackIn:
                    return BackIn;
                case EaseType.BackOut:
                    return BackOut;
                case EaseType.BackInOut:
                    return BackInOut;
                default:
                    return Linear;
            }
        }

        private const float HalfPi = Mathf.PI / 2f;

        private static readonly System.Func<float, float> Linear = (time) => time;

        private static readonly System.Func<float, float> Step = (time) =>
            time < 0.5f ? 0f : 1f;

        private static readonly System.Func<float, float> SineIn = (time) =>
            Mathf.Clamp01 (Mathf.Sin (time * HalfPi - HalfPi) + 1f);

        private static readonly System.Func<float, float> SineOut = (time) =>
            Mathf.Clamp01 (Mathf.Sin (time * HalfPi));

        private static readonly System.Func<float, float> SineInOut = (time) =>
            Mathf.Clamp01 ((Mathf.Sin (time * Mathf.PI - HalfPi) + 1f) / 2f);

        private static readonly System.Func<float, float> Pow2In = (time) =>
            Mathf.Clamp01 (Mathf.Pow (time, 2f));

        private static readonly System.Func<float, float> Pow2Out = (time) =>
            Mathf.Clamp01 (-(Mathf.Pow (time - 1f, 2f) - 1f));

        private static readonly System.Func<float, float> Pow2InOut = (time) =>
            time < 0.5f ?
            Mathf.Clamp01 (Mathf.Pow (time * 2f, 2f) * 0.5f) :
            Mathf.Clamp01 ((-(Mathf.Pow ((time * 2f) - 2f, 2f) - 1f) * 0.5f) + 0.5f);

        private static readonly System.Func<float, float> Pow3In = (time) =>
            Mathf.Clamp01 (Mathf.Pow (time, 3f));

        private static readonly System.Func<float, float> Pow3Out = (time) =>
            Mathf.Clamp01 ((Mathf.Pow (time - 1f, 3f) + 1f));

        private static readonly System.Func<float, float> Pow3InOut = (time) =>
            time < 0.5f ?
            Mathf.Clamp01 (Mathf.Pow (time * 2f, 3f) * 0.5f) :
            Mathf.Clamp01 (((Mathf.Pow ((time * 2f) - 2f, 3f) + 1f) * 0.5f) + 0.5f);

        private static readonly System.Func<float, float> Pow4In = (time) =>
            Mathf.Clamp01 (Mathf.Pow (time, 4f));

        private static readonly System.Func<float, float> Pow4Out = (time) =>
            Mathf.Clamp01 (-(Mathf.Pow (time - 1f, 4f) - 1f));

        private static readonly System.Func<float, float> Pow4InOut = (time) =>
            time < 0.5f ?
            Mathf.Clamp01 (Mathf.Pow (time * 2f, 4f) * 0.5f) :
            Mathf.Clamp01 ((-(Mathf.Pow ((time * 2f) - 2f, 4f) - 1f) * 0.5f) + 0.5f);

        private static readonly System.Func<float, float> Pow5In = (time) =>
            Mathf.Clamp01 (Mathf.Pow (time, 5f));

        private static readonly System.Func<float, float> Pow5Out = (time) =>
            Mathf.Clamp01 ((Mathf.Pow (time - 1f, 5f) + 1f));

        private static readonly System.Func<float, float> Pow5InOut = (time) =>
            time < 0.5f ?
            Mathf.Clamp01 (Mathf.Pow (time * 2f, 5f) * 0.5f) :
            Mathf.Clamp01 (((Mathf.Pow ((time * 2f) - 2f, 5f) + 1f) * 0.5f) + 0.5f);

        private static readonly System.Func<float, float> ElasticIn = (time) =>
        {
            if (time <= 0f) return 0f;
            if (time >= 1f) return 1f;

            return time * Mathf.Cos (Mathf.PI * 4f * time);
        };

        private static readonly System.Func<float, float> ElasticOut = (time) =>
        {
            if (time <= 0f) return 0f;
            if (time >= 1f) return 1f;

            return 1f - ((1f - time) * Mathf.Cos (Mathf.PI * 4f * time));
        };

        private static readonly System.Func<float, float> ElasticInOut = (time) =>
        {
            if (time <= 0f) return 0f;
            if (time >= 1f) return 1f;

            return (Mathf.Sin (time * Mathf.PI) * Mathf.Cos (Mathf.PI * time * 3f)) + time;
        };

        private static readonly System.Func<float, float> BounceIn = (time) =>
        {
            if (time <= 0f) return 0f;
            if (time >= 1f) return 1f;

            float value = time * Mathf.Cos (Mathf.PI * 3f * time);
            return value < 0f ? -value : value;
        };

        private static readonly System.Func<float, float> BounceOut = (time) =>
        {
            if (time <= 0f) return 0f;
            if (time >= 1f) return 1f;

            float value = 1f - ((1f - time) * Mathf.Cos (Mathf.PI * 3f * time));
            return value > 1f ? 2f - value : value;
        };

        private static readonly System.Func<float, float> BounceInOut = (time) =>
        {
            if (time <= 0f) return 0f;
            if (time >= 1f) return 1f;

            float value = (Mathf.Sin (time * Mathf.PI) * Mathf.Cos (Mathf.PI * 3f * time)) + time;
            return value<0f ? -value : value> 1f ? 2f - value : value;
        };

        private static readonly System.Func<float, float> BackIn = (time) =>
        {
            if (time <= 0f) return 0f;
            if (time >= 1f) return 1f;

            return time + ((1f - Mathf.Pow (time, 4f)) * -0.5f * Mathf.Sin (time * Mathf.PI));
        };

        private static readonly System.Func<float, float> BackOut = (time) =>
        {
            if (time <= 0f) return 0f;
            if (time >= 1f) return 1f;

            return time + ((1f - time) * 1.5f * Mathf.Sin (time * 2.5f));
        };

        private static readonly System.Func<float, float> BackInOut = (time) =>
        {
            if (time <= 0f) return 0f;
            if (time >= 1f) return 1f;

            return time + ((1.25f * Mathf.Sin (time * Mathf.PI)) * (.5f * Mathf.Sin (-2f * time * Mathf.PI)));
        };
    }
}