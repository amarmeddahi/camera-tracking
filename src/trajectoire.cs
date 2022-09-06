using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.CoreModule;
using System;
using System.Linq;

// Permet de stocker les coordonnees des points selectionnes a l'écran en vue d'un traitement
public class trajectoire : MonoBehaviour
{

    // Listes memoires des points de controles
    List<float> PositionsX = new List<float>();
    List<float> PositionsY = new List<float>();
    List<float> PositionsZ = new List<float>();

    // Courbes fermees
    // degres des polynomes par morceaux
    public int degres = 5;
    // nombre d'itération de subdivision
    public int nombreIteration = 5;

    // Trajectoire de la camera
    List<Vector3> Trajectoire = new List<Vector3>();

    int i = 0;

    float updateInterval = 0.05f;

    //////////////////////////////////////////////////////////////////////////
// fonction : subdivise                                                 //
// semantique : réalise nombreIteration subdivision pour des polys de   //
//              degres degres                                           //
// params : - List<float> X : abscisses des point de controle           //
//          - List<float> Y : odronnees des point de controle           //
// sortie :                                                             //
//          - (List<float>, List<float>) : points de la courbe          //
//////////////////////////////////////////////////////////////////////////
(List<float>, List<float>, List<float>) subdivise(List<float> X, List<float> Y, List<float> Z) {
        for (int p = 0; p < nombreIteration; p++)
        {
            List<float> Xinter = new List<float>();
            List<float> Yinter = new List<float>();
            List<float> Zinter = new List<float>();

            Xinter.Add(X[X.Count-1]);
            Yinter.Add(Y[X.Count-1]);
            Zinter.Add(Z[X.Count-1]);

            Xinter.Add(X[X.Count - 1]);
            Yinter.Add(Y[X.Count - 1]);
            Zinter.Add(Z[X.Count - 1]);

            Xinter.Add(X[X.Count - 1]);
            Yinter.Add(Y[X.Count - 1]);
            Zinter.Add(Z[X.Count - 1]);

            for (int i = 0; i < X.Count; i++)
            {
                Xinter.Add(X[i]);
                Xinter.Add(X[i]);
                Zinter.Add(Z[i]);
                Zinter.Add(Z[i]);
                Yinter.Add(Y[i]);
                Yinter.Add(Y[i]);
            }
            Xinter.Add(X[0]);
            Yinter.Add(Y[0]);
            Zinter.Add(Z[0]);
            X = Xinter;
            Y = Yinter;
            Z = Zinter;
            for (int k = 0; k < degres; k++)
            {
                Xinter = new List<float>();
                Yinter = new List<float>();
                Zinter = new List<float>();

                int n = X.Count;

                for (int i = 0; i < n - 1; i++)
                {
                    Xinter.Add(0.5f * X[i] + 0.5f * X[i + 1]);
                    Yinter.Add(0.5f * Y[i] + 0.5f * Y[i + 1]);
                    Zinter.Add(0.5f * Z[i] + 0.5f * Z[i + 1]);
                }

                X = Xinter;
                Y = Yinter;
                Z = Zinter;
            }
        }
        return (X, Y, Z);
    }

    // Methode appellee au debut du programme
    void Start()
    {
      // Récuperer les sphères de controle
      GameObject[] points = GameObject.FindGameObjectsWithTag("points");
      foreach (GameObject point in points)
      {
        PositionsX.Add(point.transform.position[0]);
        PositionsY.Add(point.transform.position[1]);
        PositionsZ.Add(point.transform.position[2]);
      }

      // Approximer la trajectoire
      List<float> Xres = new List<float>();
      List<float> Yres = new List<float>();
      List<float> Zres = new List<float>();
      (Xres, Yres, Zres) = subdivise(PositionsX,PositionsY, PositionsZ);
      for (int i = 0; i < Xres.Count; ++i) {
        Trajectoire.Add(new Vector3(Xres[i],Yres[i], Zres[i]));
      }

      InvokeRepeating("UpdateInterval",updateInterval,updateInterval);

    }

    // Methode appellee a chaque pas de simulation
    void UpdateInterval()
    {
        // Translater la camera sur la trajectoire
          transform.position = Trajectoire[i++ % Trajectoire.Count];

          // Orienter la camera vers le sujet
          GameObject[] target = GameObject.FindGameObjectsWithTag("sujet");
          Vector3 relativePos = target[0].transform.position - transform.position;
          Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
          transform.rotation = rotation;


      }



    void OnDrawGizmos()
        {
          Gizmos.color = Color.red;
          for (int i = 0; i < Trajectoire.Count - 1; i++)
          {
              Gizmos.DrawLine(Trajectoire[i], Trajectoire[i + 1]);
              Gizmos.DrawLine(Trajectoire[Trajectoire.Count - 1], Trajectoire[0]);
          }
        }
}
