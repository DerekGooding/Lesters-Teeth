﻿using static System.Console;

namespace Lesters_Teeth;

internal static class Program
{
    private const int ZERO = -1;
    private const int ONE = 0;
    private const int TWO = 1;
    private const int THREE = 2;
    private const int FOUR = 3;
    private const int FIVE = 4;
    private const int SIX = 5;

    private static int[]? m_Dice;
    private static int[]? m_Count;
    private static Random? m_Rnd;

    private static void SeedRng(ref string[] args)
    {
        int iSeed = 0;
        bool bSeed = false;
        if (args.Length > 0)
            bSeed = int.TryParse(args[0], out iSeed);
        m_Rnd = bSeed ? new Random(iSeed) : new Random();
    }

    private static bool IsLargeStraight(int nDice)
        => m_Count != null &&
                nDice == 6 &&
                m_Count[ONE] == 1 &&
                m_Count[TWO] == 1 &&
                m_Count[THREE] == 1 &&
                m_Count[FOUR] == 1 &&
                m_Count[FIVE] == 1 &&
                m_Count[SIX] == 1;

    private static bool Is3Pair(int nDice)
    {
        bool bIs3Pair = false;
        if (nDice == 6)
        {
            int n3Pair = 0;
            if (m_Count?[ONE] == 2)
                n3Pair++;
            if (m_Count?[TWO] == 2)
                n3Pair++;
            if (m_Count?[THREE] == 2)
                n3Pair++;
            if (m_Count?[FOUR] == 2)
                n3Pair++;
            if (m_Count?[FIVE] == 2)
                n3Pair++;
            if (m_Count?[SIX] == 2)
                n3Pair++;
            bIs3Pair = n3Pair == 3;
        }
        return bIs3Pair;
    }

    private static int RollPlayer(int iGameScorePlayer, int iPlayerID)
    {
        bool bRollLoop = true;
        int nDice = 6;
        const int nRoll = 0;
        int iRollScore = 0;
        bool bBuela = false;
        int nMult = ZERO;
        while (bRollLoop)
        {
            while (true)
            {
                if (iRollScore >= 500 || iGameScorePlayer >= 500)
                    WriteLine("Press R to roll, S to score");
                else
                    WriteLine("Press R to roll");
                ConsoleKeyInfo cki = ReadKey(true);
                string strKey = cki.KeyChar.ToString();
                if (iRollScore >= 500 || iGameScorePlayer >= 500)
                {
                    if (strKey == "S" || strKey == "s")
                    {
                        bRollLoop = false;
                        break;
                    }
                }
                if (strKey == "R" || strKey == "r")
                    break;
            }

            if (!bRollLoop)
                break;

            Write(" Roll {0}: ", nRoll + 1);
            int iTempScore = 0;
            m_Dice = [0, 0, 0, 0, 0, 0];
            m_Count = [0, 0, 0, 0, 0, 0];
            for (int iDie = 0; iDie < nDice; iDie++)
            {
                m_Dice[iDie] = m_Rnd.Next(1, 7);
                m_Count[m_Dice[iDie] - 1]++;
                Write("{0} ", m_Dice[iDie]);
            }
            WriteLine();

#if DEBUG
            const bool bWriteCount = true;
            if (bWriteCount)
            {
                Write("Count {0}: ", nRoll + 1);
                for (int iDie = 0; iDie < 6; iDie++)
                    Write("{0} ", m_Count[iDie]);
                WriteLine();
            }
            WriteLine();
#endif

            bBuela = true;
            if (IsLargeStraight(nDice) || Is3Pair(nDice))
            {
                bBuela = false;
            }
            else
            {
                if (nMult == ONE)
                {
                    if (m_Count[ONE] > 0)
                        bBuela = false;
                }
                else if (m_Count[ONE] > 2)
                {
                    bBuela = false;
                }
                else if (nMult == SIX)
                {
                    if (m_Count[SIX] > 0)
                        bBuela = false;
                }
                else if (m_Count[SIX] >= 3)
                {
                    bBuela = false;
                }
                else if (nMult == FIVE)
                {
                    if (m_Count[FIVE] > 0)
                        bBuela = false;
                }
                else if (m_Count[FIVE] >= 3)
                {
                    bBuela = false;
                }
                else if (nMult == FOUR)
                {
                    if (m_Count[FOUR] > 0)
                        bBuela = false;
                }
                else if (m_Count[FOUR] >= 3)
                {
                    bBuela = false;
                }
                else if (nMult == THREE)
                {
                    if (m_Count[THREE] > 0)
                        bBuela = false;
                }
                else if (m_Count[THREE] >= 3)
                {
                    bBuela = false;
                }
                else if (nMult == TWO)
                {
                    if (m_Count[TWO] > 0)
                        bBuela = false;
                }
                else if (m_Count[TWO] >= 3)
                {
                    bBuela = false;
                }

                if (m_Count[ONE] > 0 && m_Count[ONE] < 3)
                    bBuela = false;
                if (m_Count[FIVE] > 0 && m_Count[FIVE] < 3)
                    bBuela = false;
            }

            if (bBuela)
            {
                bRollLoop = false;
                WriteLine("Buela!");
                bBuela = true;
            }
            else
            {
                do
                {
                    bool bChoose = true;
                    do
                    {
                        int[] Count = [m_Count[0], m_Count[1], m_Count[2], m_Count[3], m_Count[4], m_Count[5]];
                        int[] Count2 = [0, 0, 0, 0, 0, 0];

                        Write("Pick up score dice: ");
                        string? strDice = ReadLine();
                        if (strDice != null)
                        {
                            bool bError = false;
                            foreach (char cDie in strDice)
                            {
                                if (cDie != ' ')
                                {
                                    int iDie = cDie - '0' - 1;
                                    if (iDie >= 0 && iDie <= 5)
                                    {
                                        if (Count[iDie] > 0)
                                        {
                                            Count[iDie]--;
                                            Count2[iDie]++;
                                        }
                                        else
                                        {
                                            bError = true;
                                        }
                                    }
                                    else
                                    {
                                        bError = true;
                                    }
                                }
                            }
                            if (!bError)
                            {
                                for (int i = 0; i < 6; i++)
                                    m_Count[i] = Count2[i];
                                bChoose = false;
                            }
                        }
                    } while (bChoose);

                    // Look for patterns
                    bool bIsLargeStraight = IsLargeStraight(nDice);
                    if (bIsLargeStraight)
                    {
                        // Large straight : 1500 points
                        iTempScore += 1500;
                        WriteLine("Roll {0} - Large Straight - Total {1}", nRoll + 1, iTempScore);
                        for (int iDie = ONE; iDie <= SIX; iDie++)
                            m_Count[iDie] = 0;
                        nDice = 0;
                        nMult = ZERO;
                    }
                    else
                    {
                        bool bIs3Pair = Is3Pair(nDice);
                        if (bIs3Pair)
                        {
                            // 3 Pair : 500 points
                            iTempScore += 500;
                            WriteLine("Roll {0} - Three Pair - Total {1}", nRoll + 1, iTempScore);
                            for (int iDie = ONE; iDie <= SIX; iDie++)
                                m_Count[iDie] = 0;
                            nDice = 0;
                            nMult = ZERO;
                        }
                        else
                        {
                            if (nMult == ONE)
                            {
                                if (m_Count[ONE] > 0)
                                {
                                    iTempScore += m_Count[ONE] * 1000;
                                    WriteLine("Roll {0} - {1} Ones - Total {2}", nRoll + 1, m_Count[ONE], iTempScore);
                                    nDice -= m_Count[ONE];
                                    m_Count[ONE] = 0;
                                }
                                else
                                {
                                    nMult = ZERO;
                                }
                            }
                            else if (m_Count[ONE] >= 3)
                            {
                                // Eintausend - 1000
                                iTempScore += (m_Count[ONE] - 2) * 1000;
                                WriteLine("Roll {0} - {1} Ones - Total {2}", nRoll + 1, m_Count[ONE], iTempScore);
                                nDice -= m_Count[ONE];
                                m_Count[ONE] = 0;
                                nMult = ONE;
                            }
                            else if (nMult == SIX)
                            {
                                if (m_Count[SIX] > 0)
                                {
                                    iTempScore += m_Count[SIX] * 600;
                                    WriteLine("Roll {0} - {1} Sixes - Total {2}", nRoll + 1, m_Count[SIX], iTempScore);
                                    nDice -= m_Count[SIX];
                                    m_Count[SIX] = 0;
                                }
                                else
                                {
                                    nMult = ZERO;
                                }
                            }
                            else if (m_Count[SIX] >= 3)
                            {
                                iTempScore += (m_Count[SIX] - 2) * 600;
                                WriteLine("Roll {0} - {1} Sixes - Total {2}", nRoll + 1, m_Count[SIX], iTempScore);
                                nDice -= m_Count[SIX];
                                m_Count[SIX] = 0;
                                nMult = SIX;
                            }
                            else if (nMult == FIVE)
                            {
                                if (m_Count[FIVE] > 0)
                                {
                                    iTempScore += m_Count[FIVE] * 500;
                                    WriteLine("Roll {0} - {1} Five(s) - Total {2}", nRoll + 1, m_Count[FIVE], iTempScore);
                                    nDice -= m_Count[FIVE];
                                    m_Count[FIVE] = 0;
                                }
                                else
                                {
                                    nMult = ZERO;
                                }
                            }
                            else if (m_Count[FIVE] >= 3)
                            {
                                iTempScore += (m_Count[FIVE] - 2) * 500;
                                WriteLine("Roll {0} - {1} Five(s) - Total {2}", nRoll + 1, m_Count[FIVE], iTempScore);
                                nDice -= m_Count[FIVE];
                                m_Count[FIVE] = 0;
                                nMult = FIVE;
                            }
                            else if (nMult == FOUR)
                            {
                                if (m_Count[FOUR] > 0)
                                {
                                    iTempScore += m_Count[FOUR] * 400;
                                    WriteLine("Roll {0} - {1} Four(s) - Total {2}", nRoll + 1, m_Count[FOUR], iTempScore);
                                    nDice -= m_Count[FOUR];
                                    m_Count[FOUR] = 0;
                                }
                                else
                                {
                                    nMult = ZERO;
                                }
                            }
                            else if (m_Count[FOUR] >= 3)
                            {
                                iTempScore += (m_Count[FOUR] - 2) * 400;
                                WriteLine("Roll {0} - {1} Four(s) - Total {2}", nRoll + 1, m_Count[FOUR], iTempScore);
                                nDice -= m_Count[FOUR];
                                m_Count[FOUR] = 0;
                                nMult = FOUR;
                            }
                            else if (nMult == THREE)
                            {
                                if (m_Count[THREE] > 0)
                                {
                                    iTempScore += m_Count[THREE] * 300;
                                    WriteLine("Roll {0} - {1} Three(s) - Total {2}", nRoll + 1, m_Count[THREE], iTempScore);
                                    nDice -= m_Count[THREE];
                                    m_Count[THREE] = 0;
                                }
                                else
                                {
                                    nMult = ZERO;
                                }
                            }
                            else if (m_Count[THREE] >= 3)
                            {
                                iTempScore += (m_Count[THREE] - 2) * 300;
                                WriteLine("Roll {0} - {1} Three(s) - Total {2}", nRoll + 1, m_Count[THREE], iTempScore);
                                nDice -= m_Count[THREE];
                                m_Count[THREE] = 0;
                                nMult = THREE;
                            }
                            else if (nMult == TWO)
                            {
                                if (m_Count[TWO] > 0)
                                {
                                    iTempScore += m_Count[TWO] * 200;
                                    WriteLine("Roll {0} - {1} Two(s) - Total {2}", nRoll + 1, m_Count[TWO], iTempScore);
                                    nDice -= m_Count[TWO];
                                    m_Count[TWO] = 0;
                                }
                                else
                                {
                                    nMult = ZERO;
                                }
                            }
                            else if (m_Count[TWO] >= 3)
                            {
                                iTempScore += (m_Count[TWO] - 2) * 200;
                                WriteLine("Roll {0} - {1} Two(s) - Total {2}", nRoll + 1, m_Count[TWO], iTempScore);
                                nDice -= m_Count[TWO];
                                m_Count[TWO] = 0;
                                nMult = TWO;
                            }

                            if (m_Count[ONE] > 0 && m_Count[ONE] < 3)
                            {
                                iTempScore += m_Count[ONE] * 100;
                                WriteLine("Roll {0} - 100 - Score {1} - Total {2}", nRoll + 1, m_Count[0] * 100, iTempScore);
                                nDice -= m_Count[ONE];
                                m_Count[ONE] = 0;
                                nMult = ZERO;
                            }
                            if (m_Count[FIVE] > 0 && m_Count[FIVE] < 3)
                            {
                                iTempScore += m_Count[FIVE] * 50;
                                WriteLine("Roll {0} - 50 - Score {1} - Total {2}", nRoll + 1, m_Count[0] * 50, iTempScore);
                                nDice -= m_Count[FIVE];
                                m_Count[FIVE] = 0;
                                nMult = ZERO;
                            }
                        }
                    }

                    if (iTempScore == 0)
                    {
                        WriteLine("Please pick the scoring dice");
                    }
                    else
                    {
                        if (nDice == 0)
                        {
                            nDice = 6;
                            nMult = ZERO;
                        }
                    }
                } while (iTempScore == 0);

                iRollScore += iTempScore;
                WriteLine("\r\nTotal Roll Score {0}\r\n", iRollScore);

                if (nMult > ZERO)
                    WriteLine("{0} is the roll multiplier\r\n", nMult + 1);
            }
        }

        if (!bBuela)
            iGameScorePlayer += iRollScore;
        WriteLine(string.Format("Player {0} - Score {1}\r\n", iPlayerID, iGameScorePlayer));
        return iGameScorePlayer;
    }

    private static int RollCPU(int iGameScoreCPU, int iCPUID)
    {
        bool bRollLoop = true;
        int nDice = 6;
        int nRoll = 0;
        int iRollScore = 0;
        bool bBuela = false;
        int nMult = ZERO;
        while (bRollLoop)
        {
            Write(" Roll {0}: ", nRoll + 1);
            int iTempScore = 0;
            m_Dice = [0, 0, 0, 0, 0, 0];
            m_Count = [0, 0, 0, 0, 0, 0];
            for (int iDie = 0; iDie < nDice; iDie++)
            {
                m_Dice[iDie] = m_Rnd.Next(1, 7);
                m_Count[m_Dice[iDie] - 1]++;
                Write("{0} ", m_Dice[iDie]);
            }
            WriteLine();

#if DEBUG
            const bool bWriteCount = true;
            if (bWriteCount)
            {
                Write("Count {0}: ", nRoll + 1);
                for (int iDie = 0; iDie < 6; iDie++)
                    Write("{0} ", m_Count[iDie]);
                WriteLine();
            }
            WriteLine();
#endif

            bool bIsLargeStraight = IsLargeStraight(nDice);
            bool bIs3Pair = Is3Pair(nDice);

            // Look for patterns
            if (bIsLargeStraight)
            {
                // Large straight : 1500 points
                iTempScore += 1500;
                WriteLine("Roll {0} - Large Straight - Total {1}", nRoll + 1, iTempScore);
                for (int iDie = ONE; iDie <= SIX; iDie++)
                    m_Count[iDie] = 0;
                nDice = 0;
                nMult = ZERO;
            }
            else
            {
                if (bIs3Pair)
                {
                    // 3 Pair : 500 points
                    iTempScore += 500;
                    WriteLine("Roll {0} - Three Pair - Total {1}", nRoll + 1, iTempScore);
                    for (int iDie = ONE; iDie <= SIX; iDie++)
                        m_Count[iDie] = 0;
                    nDice = 0;
                    nMult = ZERO;
                }
                else
                {
                    if (nMult == ONE)
                    {
                        if (m_Count[ONE] > 0)
                        {
                            iTempScore += m_Count[ONE] * 1000;
                            WriteLine("Roll {0} - {1} Ones - Total {2}", nRoll + 1, m_Count[ONE], iTempScore);
                            nDice -= m_Count[ONE];
                            m_Count[ONE] = 0;
                        }
                        else
                        {
                            nMult = ZERO;
                        }
                    }
                    else if (m_Count[ONE] >= 3)
                    {
                        // Eintausend - 1000
                        iTempScore += (m_Count[ONE] - 2) * 1000;
                        WriteLine("Roll {0} - {1} Ones - Total {2}", nRoll + 1, m_Count[ONE], iTempScore);
                        nDice -= m_Count[ONE];
                        m_Count[ONE] = 0;
                        nMult = ONE;
                    }
                    else if (nMult == SIX)
                    {
                        if (m_Count[SIX] > 0)
                        {
                            iTempScore += m_Count[SIX] * 600;
                            WriteLine("Roll {0} - {1} Sixes - Total {2}", nRoll + 1, m_Count[SIX], iTempScore);
                            nDice -= m_Count[SIX];
                            m_Count[SIX] = 0;
                        }
                        else
                        {
                            nMult = ZERO;
                        }
                    }
                    else if (m_Count[SIX] >= 3)
                    {
                        iTempScore += (m_Count[SIX] - 2) * 600;
                        WriteLine("Roll {0} - {1} Six(es) - Total {2}", nRoll + 1, m_Count[SIX], iTempScore);
                        nDice -= m_Count[SIX];
                        m_Count[SIX] = 0;
                        nMult = SIX;
                    }
                    else if (nMult == FIVE)
                    {
                        if (m_Count[FIVE] > 0)
                        {
                            iTempScore += m_Count[FIVE] * 500;
                            WriteLine("Roll {0} - {1} Five(s) - Total {2}", nRoll + 1, m_Count[FIVE], iTempScore);
                            nDice -= m_Count[FIVE];
                            m_Count[FIVE] = 0;
                        }
                        else
                        {
                            nMult = ZERO;
                        }
                    }
                    else if (m_Count[FIVE] >= 3)
                    {
                        iTempScore += (m_Count[FIVE] - 2) * 500;
                        WriteLine("Roll {0} - {1} Five(s) - Total {2}", nRoll + 1, m_Count[FIVE], iTempScore);
                        nDice -= m_Count[FIVE];
                        m_Count[FIVE] = 0;
                        nMult = FIVE;
                    }
                    else if (nMult == FOUR)
                    {
                        if (m_Count[FOUR] > 0)
                        {
                            iTempScore += m_Count[FOUR] * 400;
                            WriteLine("Roll {0} - {1} Four(s) - Total {2}", nRoll + 1, m_Count[FOUR], iTempScore);
                            nDice -= m_Count[FOUR];
                            m_Count[FOUR] = 0;
                        }
                        else
                        {
                            nMult = ZERO;
                        }
                    }
                    else if (m_Count[FOUR] >= 3)
                    {
                        iTempScore += (m_Count[FOUR] - 2) * 400;
                        WriteLine("Roll {0} - {1} Four(s) - Total {2}", nRoll + 1, m_Count[FOUR], iTempScore);
                        nDice -= m_Count[FOUR];
                        m_Count[FOUR] = 0;
                        nMult = FOUR;
                    }
                    else if (nMult == THREE)
                    {
                        if (m_Count[THREE] > 0)
                        {
                            iTempScore += m_Count[THREE] * 300;
                            WriteLine("Roll {0} - {1} Three(s) - Total {2}", nRoll + 1, m_Count[THREE], iTempScore);
                            nDice -= m_Count[THREE];
                            m_Count[THREE] = 0;
                        }
                        else
                        {
                            nMult = ZERO;
                        }
                    }
                    else if (m_Count[THREE] >= 3)
                    {
                        iTempScore += (m_Count[THREE] - 2) * 300;
                        WriteLine("Roll {0} - {1} Three(s) - Total {2}", nRoll + 1, m_Count[THREE], iTempScore);
                        nDice -= m_Count[THREE];
                        m_Count[THREE] = 0;
                        nMult = THREE;
                    }
                    else if (nMult == TWO)
                    {
                        if (m_Count[TWO] > 0)
                        {
                            iTempScore += m_Count[TWO] * 200;
                            WriteLine("Roll {0} - {1} Two(s) - Total {2}", nRoll + 1, m_Count[TWO], iTempScore);
                            nDice -= m_Count[TWO];
                            m_Count[TWO] = 0;
                        }
                        else
                        {
                            nMult = ZERO;
                        }
                    }
                    else if (m_Count[TWO] >= 3)
                    {
                        iTempScore += (m_Count[TWO] - 2) * 200;
                        WriteLine("Roll {0} - {1} Two(s) - Total {2}", nRoll + 1, m_Count[TWO], iTempScore);
                        nDice -= m_Count[TWO];
                        m_Count[TWO] = 0;
                        nMult = TWO;
                    }

                    if (m_Count[ONE] > 0 && m_Count[ONE] < 3)
                    {
                        iTempScore += m_Count[ONE] * 100;
                        WriteLine("Roll {0} - {1} Ones(s) - Total {2}", nRoll + 1, m_Count[ONE], iTempScore);
                        nDice -= m_Count[ONE];
                        m_Count[ONE] = 0;
                        nMult = ZERO;
                    }

                    if (m_Count[FIVE] > 0 && m_Count[FIVE] < 3)
                    {
                        iTempScore += m_Count[FIVE] * 50;
                        WriteLine("Roll {0} - {1} Five(s) - Total {2}", nRoll + 1, m_Count[FIVE], iTempScore);
                        nDice -= m_Count[FIVE];
                        m_Count[FIVE] = 0;
                        nMult = ZERO;
                    }
                }
            }

            if (iTempScore == 0)
            {
                bRollLoop = false;
                WriteLine("Buela!");
                bBuela = true;
            }
            else
            {
                iRollScore += iTempScore;
                WriteLine("Total Roll Score {0}", iRollScore);

                if (nMult > ZERO)
                    WriteLine("{0} is the roll multiplier", nMult + 1);

                int iChance = m_Rnd.Next(1, 101);
                if (iGameScoreCPU < 500)
                {
                    if (iGameScoreCPU + iRollScore >= 500)
                    {
                        if (nDice > 0)
                        {
                            if (nMult > ZERO)
                            {
                                // Could have 3, 2, or 1 dice left.  Higher multiplier makes higher score. Determine chance of going for it!
                                if (nMult == ONE || nMult == FIVE || nMult == SIX)
                                {
                                    int GT = nMult == ONE ? 80 : (nMult == FIVE ? 90 : 100);
                                    if ((iChance * nDice) < GT)
                                    {
                                        WriteLine("Not going for it! It's Your turn");
                                        bRollLoop = false;
                                    }
                                    else
                                    {
                                        WriteLine("Going for it!");
                                    }
                                }
                                else
                                {
                                    if (iChance < 90)
                                    {
                                        WriteLine("Not going for it! It's Your turn");
                                        bRollLoop = false;
                                    }
                                    else
                                    {
                                        WriteLine("Going for it!");
                                    }
                                }
                            }
                            else
                            {
                                WriteLine("On the board! It's Your turn");
                                bRollLoop = false;
                            }
                        }
                        else
                        {
                            if (iChance > 75)
                            {
                                WriteLine("Going for it, Fresh Teeth!");
                                nDice = 6;
                            }
                            else
                            {
                                WriteLine("Not going for it. On the board! It's Your turn");
                                bRollLoop = false;
                            }
                        }
                    }
                }
                else
                {
                    if (nDice == 0)
                    {
                        if (iRollScore > 2000)
                        {
                            WriteLine("Fresh Teeth, but not going for it. Your turn");
                            bRollLoop = false;
                        }
                        else
                        {
                            WriteLine("Fresh Teeth");
                            nDice = 6;
                        }
                    }
                    else
                    {
                        if (iRollScore > 500 && nDice <= 2)
                        {
                            WriteLine("Your turn");
                            bRollLoop = false;
                        }

                        if (bRollLoop)
                            WriteLine("Go Teeth!");
                    }
                    nRoll++;
                }
                WriteLine();
            }
            Thread.Sleep(1000);
        }
        if (!bBuela)
            iGameScoreCPU += iRollScore;
        WriteLine(string.Format("CPU {0} - Score {1}\r\n", iCPUID, iGameScoreCPU));
        return iGameScoreCPU;
    }

    private static void Scoring()
    {
        WriteLine("Lesters Teeth");
        WriteLine();
        WriteLine("Scoring:");
        WriteLine("Straight - 1500");
        WriteLine("3 Ones   - 1000 - Additional 1000 each");
        WriteLine("3 Sixes  - 600  - Additional 600 each");
        WriteLine("3 Fives  - 500  - Additional 500 each");
        WriteLine("3 Fours  - 400  - Additional 400 each");
        WriteLine("3 Threes - 300  - Additional 300 each");
        WriteLine("3 Twos   - 200  - Additional 200 each");
        WriteLine("3 Pair   - 500");
        WriteLine("One      - 100");
        WriteLine("Five     - 50");
        WriteLine();
    }

    private static void Main(string[] args)
    {
        SeedRng(ref args);

        bool bAgain = true;

        while (bAgain)
        {
            Scoring();

            bool bConvert = false;
            int nPlayers = 0, nCPUs = 0, nPeople = 0;
            do
            {
                Write("Number of players? ");
                string? strPlayers = ReadLine();
                bConvert = int.TryParse(strPlayers, out nPlayers);
                if (bConvert)
                {
                    if (nPlayers < 2)
                    {
                        WriteLine("Not enough players");
                        bConvert = false;
                    }
                    else
                    {
                        Write("How many are CPU? ");
                        string? strCPUs = ReadLine();
                        bConvert = int.TryParse(strCPUs, out nCPUs);
                        if (!bConvert)
                        {
                            WriteLine("Input error, try again");
                        }
                        else
                        {
                            if (nCPUs < 0)
                            {
                                WriteLine("There must be 0 or more CPU players");
                                bConvert = false;
                            }
                            else
                            {
                                if (nCPUs > nPlayers)
                                {
                                    WriteLine("Too many CPUs");
                                    bConvert = false;
                                }
                                else
                                {
                                    nPeople = nPlayers - nCPUs;
                                }
                            }
                        }
                    }
                }
                else
                {
                    WriteLine("Input error, try again");
                }
            } while (!bConvert);

            WriteLine("\r\nStarting order is Player(s) first, CPU(s) second. Good Luck!\r\n");

            int[] PlayerScore = new int[nPlayers];

            bool bGameLoop = true;
            while (bGameLoop)
            {
                WriteLine("Lesters Teeth\r\n");
                for (int iPlayer = 0; iPlayer < nPlayers; iPlayer++)
                {
                    if (iPlayer < nPeople)
                        PlayerScore[iPlayer] = RollPlayer(PlayerScore[iPlayer], iPlayer + 1);
                    else
                        PlayerScore[iPlayer] = RollCPU(PlayerScore[iPlayer], iPlayer + 1);

                    if (PlayerScore[iPlayer] > 10000)
                    {
                        bGameLoop = false;
                        break;
                    }
                }
            }

            WriteLine();
            for (int iPlayer = 0; iPlayer < nPlayers; iPlayer++)
            {
                if (PlayerScore[iPlayer] > 10000)
                {
                    if (iPlayer < nPeople)
                        WriteLine("Player {0} scores {1} and wins!", iPlayer + 1, PlayerScore[iPlayer]);
                    else
                        WriteLine("CPU {0} scores {1} and wins!", iPlayer + 1 - nPeople, PlayerScore[iPlayer]);
                }
                else
                {
                    if (iPlayer < nPeople)
                        WriteLine("Player {0} scores {1} and loses!", iPlayer + 1, PlayerScore[iPlayer]);
                    else
                        WriteLine("CPU {0} scores {1} and loses!", iPlayer + 1 - nPeople, PlayerScore[iPlayer]);
                }
            }

            WriteLine();
            WriteLine("Press A to play again.\r\n");
            ConsoleKeyInfo cki = ReadKey(true);
            string strKey = cki.KeyChar.ToString();
            if (strKey != "A" && strKey != "a")
                bAgain = false;
        }
    }
}