namespace Tests;

[TestClass]
public class IEnumerableTest
{

    [TestMethod]
    public void Extensions()
    {
        var chaine = "lkjdflqsjqsl jklqskj dljk qsdljk qsjkl jkl q";
        var chaineRaccourcie = MesExtensions.Ellipsis(chaine,10); // lkjdflq...
        // Syntaxic sugar
        chaineRaccourcie = chaine.Ellipsis(10);
        
        // Obtenir les nombres par tableau de 10
        var numbers = Fibo().Buffer(10);

        foreach(var tab in numbers)
        {

        }
    }

    [TestMethod]
    public void Enumeration()
    {
        // IEnumerable => G�n�rator
        // Mat�rialisation possible avec ToList ou ToArray => Effectue l'it�ration et stocke les r�sultats en m�moire 
        // Enumeration lorsque une first,last a r�ellement besoin de l'info
        var fiboNumbers = Fibo().Where(c=>c%2==0).Reverse();

        var entiers = new List<int>() { 7, 3, 6, 3, 2, 8, 10,6,6,3 }; // 1 000 0000
        // Les fonctions de Linq sont �vlau�es de mani�re lasy
        var petitsEntiers = entiers.Where(c =>
            c >7 ).ToList(); // 200 0000

        entiers.Add(10);
        var count = petitsEntiers.Count(); //2

        if (count > 0)
        {
            var enumerator = petitsEntiers.GetEnumerator();
            while (enumerator.MoveNext()) {
                var i = enumerator.Current;
            }

            foreach (var i in Fibo().Take(1000))
            {
                if (i > 100)
                {
                    break;
                }
                Int128 a = i;
            }
        }
    }

    // 1 1 2 3 5 8 13 21
    // Fibo retourne la liste des �l�ments de la suite de fibonachi
    IEnumerable<Int128> Fibo()
    {
        //return new Int128[] { 1, 2, 3, 4 };
        Int128 precedent= 0;
        Int128 courant = 1;
        // Dans ce cas, le g�n�rateur est pr�t a continuer � l'infini
        // Responsabilit� d'e l'iterateur de s'arr�ter
        while (true) {
            yield return courant;
            var suivant = precedent + courant;
            precedent = courant;
            courant = suivant;
        }

    }

}

