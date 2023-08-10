using Infrastructure.BaseMonoCache.Code.MonoCache;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Observer
{
    public class MemberFrame : MonoCache
    {
        [SerializeField] private Image _iconMember;
        
        [SerializeField] private TMP_Text _fractionMember;
        [SerializeField] private TMP_Text _nameMember;
        [SerializeField] private TMP_Text _levelMember;
        
        [SerializeField] private TMP_Text _countSurvivals;
        [SerializeField] private TMP_Text _countGetDamage;
        [SerializeField] private TMP_Text _countTakeDamage;
        
        
        public void Init(Image iconMember, string fractionMember, 
            string nameMember, string levelMember, 
            string countSurvivals, string countGetDamage, 
            string countTakeDamage)
        {
            _iconMember = iconMember;
            _fractionMember.text = fractionMember;
            _nameMember.text = nameMember;
            _levelMember.text = levelMember;
            _countSurvivals.text = countSurvivals;
            _countGetDamage.text = countGetDamage;
            _countTakeDamage.text = countTakeDamage;
        }
    }
}