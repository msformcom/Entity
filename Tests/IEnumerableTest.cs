namespace Tests;

[TestClass]
public class IEnumerableTest
{
    [TestMethod]
    public void Enumeration()
    {
        // IEnumerable => Générator
        // Matérialisation possible avec ToList ou ToArray => Effectue l'itération et stocke les résultats en mémoire 
        // Enumeration lorsque une first,last a réellement besoin de l'info
        var fiboNumbers = Fibo().Where(c=>c%2==0).Reverse();

        var entiers = new List<int>() { 7, 3, 6, 3, 2, 8, 10,6,6,3 }; // 1 000 0000
        // Les fonctions de Linq sont évlauées de manière lasy
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
    // Fibo retourne la liste des éléments de la suite de fibonachi
    IEnumerable<Int128> Fibo()
    {
        //return new Int128[] { 1, 2, 3, 4 };
        Int128 precedent= 0;
        Int128 courant = 1;
        // Dans ce cas, le générateur est prêt a continuer à l'infini
        // Responsabilité d'e l'iterateur de s'arrêter
        while (true) {
            yield return courant;
            var suivant = precedent + courant;
            precedent = courant;
            courant = suivant;
        }

    }

}