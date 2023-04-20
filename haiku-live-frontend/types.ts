export interface Topic {
  id: number
  name: string
}

export interface Author {
  id: number
  name: string
}

export interface Haiku {
  id: number
  line1: string
  line2: string
  line3: string
  authorId: number
  authorName: string
  topicId: number
}

export interface NewHaiku {
  line1: string
  line2: string
  line3: string
  authorId: number
  authorName: string
  topicId: number
}
