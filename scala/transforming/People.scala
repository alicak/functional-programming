package transforming

object People {
  def loadPeople: List[String] = {
    val wordstream = Option {
      getClass.getResourceAsStream("/files/people-errors.csv")
    } getOrElse {
      sys.error("Could not load people, file not found")
    }
    try {
      val s = scala.io.Source.fromInputStream(wordstream)
      s.getLines.toList
    } catch {
      case e: Exception =>
        println("Could not load people: " + e)
        throw e
    } finally {
      wordstream.close()
    }
  }
}
