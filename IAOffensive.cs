﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Miniville
{
    public class IAOffensive : IA
    {
        private bool expert;
        private int moneyToWin;
        public IAOffensive(Player _player, int _mode) : base(_player)
        {
            expert = _mode == 3;
            if (expert)
            {
                moneyToWin = 20;
            }
            else
            {
                moneyToWin = (_mode + 1) * 10;
            }
        }

        public override int IANbDice()
        {
            int oneDiceScore = 0;
            int twoDiceScore = 0;

            foreach (Card card in player.city)
            {
                int saveFirstCond = 0;
                foreach (int i in card.dieCondition)
                {
                    if (saveFirstCond != i)
                    {
                        //Si inférieur ou égal à 5, renforcer le score de choix pour un dé selon le gain possible, sinon pour 2
                        if (i <= 5)
                            oneDiceScore += card.moneyToEarn;
                        else
                            twoDiceScore += card.moneyToEarn;
                        saveFirstCond = i;
                    }
                    
                }
            }
            int nbDice = oneDiceScore > twoDiceScore ? 1 : 2;
            return nbDice;
        }

        protected override Pile Choose(List<Pile> _possiblePiles)
        {
            //1 chance sur 5 de ne pas acheter
            if (random.Next(0,5)==0)
                return null;
            
            foreach (Pile pile in _possiblePiles)
            {
                //Si on n'a pas la carte et qu'on est en expert, l'acheter
                if (!player.city.Contains(pile.card) && expert)
                    return pile;
                
                //Sinon si on a beaucoup d'argent, économiser
                if (player.pieces > moneyToWin*0.7f && !expert)
                    return null;
                
                foreach (int i in pile.card.dieCondition)
                {
                    //Si on ne couvre que peu le lancé de dé, choisir cette pile
                    if (CoveredDiceRoll()[i-1] == CoveredDiceRoll().Min())
                    {
                        return pile;
                    }
                }
            }
            //S'il n'y a pas eu de retour précédent, acheter une carte aléatoire
            return _possiblePiles[random.Next(_possiblePiles.Count)];
        }
    }
}